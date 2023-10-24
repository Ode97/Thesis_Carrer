using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Character : MonoBehaviour
{
    public int health;

    private Inventory inventory;

    private GameObject aurea;

    private MagicElement actualElement;
    private InteractableObject obj;

    private Movment movment;
    private Animator animator;
    private bool activeElement = false;
    private GameObject[] hearts;
    private Enemy enemyTarget;
    private bool attacking = false;
    [SerializeField]
    private Canvas mainCanvas;
    private float space = 110;
    private GameObject water;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        movment = GetComponent<Movment>();
        hearts = new GameObject[health];
        
        Reset();
    }

    private void Reset()
    {
        var h = Resources.Load<GameObject>("life");
        health = 3;

        Vector2 canvasSize = mainCanvas.GetComponent<RectTransform>().sizeDelta;

        for (int i = 0; i < health; i++)
        {
            GameObject heart = Instantiate(h, Vector3.zero, Quaternion.identity, mainCanvas.transform);
            RectTransform rt = heart.GetComponent<RectTransform>();

            // Calculate the position based on canvas size
            float posX = -canvasSize.x / 2 + space * i + 65;
            float posY = -canvasSize.y / 2 + 65;

            rt.anchoredPosition = new Vector2(posX, -posY); // Negative y to account for Unity's UI system

            hearts[i] = heart;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (water)
        {
            WaterRespawn();
        }

        if(health <= 0)
        {
            StartCoroutine(Dead());
            
        }
        if(enemyTarget)
            Attack();
    }

    private IEnumerator Dead()
    {
        GameManager.instance.stopLogic = true;
        animator.SetTrigger("Die");
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Die01_Stay_SwordAndShield"));
        GameManager.instance.stopLogic = true;
        animator.ResetTrigger("Die");
        yield return new WaitForSeconds(3);
        Reset();
        Respawn();
        animator.Play("Idle_Battle_SwordAndShield");
    }

    public void SetEnemy(Enemy e)
    {
        enemyTarget = e;
    }

    public void StopImmediateAurea()
    {
        
        if (aurea != null)
        {
            Destroy(aurea.gameObject);
            activeElement = false;
        }
    }

    public void MoveToDestination(Vector3 point)
    {
        movment.Move(point);
    }

    public void SetActualElement(MagicElement element)
    {
        if(aurea != null)
            Destroy(aurea.gameObject);


        if (activeAttack)
        {
            Destroy(activeAttack);
        }

        if (actualElement != null && element == actualElement)
        {
            actualElement = null;
            activeElement = false;
            return;
        }

        

        activeElement = true;

        aurea = Instantiate(element.aurea, transform);
        actualElement = element;
    }

    public bool isActiveElement()
    {
        return activeElement;
    }

    public MagicElement getActualElement()
    {
        return actualElement;
    }

    public void SetObject(InteractableObject obj)
    {
        this.obj = obj;
        //Debug.Log(obj.gameObject.name);
    }

    public InteractableObject GetObject()
    {
        return obj;
    }

    public void Interaction(RaycastHit hit)
    {
        if (activeElement)
        {           
            actualElement.SetObject(obj);
            actualElement.SetPosition(hit.point);
            actualElement.ApplyEffect();
        }
        else
        {
            obj.Interaction();
        }
    }

    private GameObject activeAttack;
    public void Attack()
    {
        
        if (activeElement && enemyTarget && !attacking && !activeAttack)
        {
            //StartCoroutine(AttackRoutine());
            activeAttack = Instantiate(actualElement.BaseAttack, transform.position + new Vector3(0, 3, 0) + transform.forward * 3, Quaternion.identity);

        }
        else
        {
            //Vector3 velocityBullet = enemyTarget.transform.position - transform.position;
            //Quaternion desiredRotation = Quaternion.LookRotation(velocityBullet.normalized, Vector3.up);
            activeAttack.transform.LookAt(enemyTarget.transform);
        }
    }

    public float bulletVelocity = 10;
    public float attackSpeed = 5;
    private IEnumerator AttackRoutine()
    {
        attacking = true;
        yield return new WaitForSeconds(10 / attackSpeed);
        if (activeElement && enemyTarget)
        {
            Vector3 velocityBullet = enemyTarget.transform.position - transform.position;
            var b = Instantiate(actualElement.BaseAttack, transform.position + new Vector3(0, 3, 0), Quaternion.identity);
            b.transform.localScale = new Vector3(1f, 1f, 1f);
            Quaternion desiredRotation = Quaternion.LookRotation(velocityBullet.normalized, Vector3.up) * Quaternion.Euler(90, 0, 0);
            b.transform.localRotation = desiredRotation;

            b.GetComponent<Rigidbody>().velocity = velocityBullet.normalized * bulletVelocity;

        }
        attacking = false;
    }

    private Vector3 checkpoint;
    private void OnCollisionEnter(Collision collision)
    {       

        if (collision.collider.gameObject.GetComponentInParent<Enemy>() && health > 0)
        {
            health -= 1;
            Destroy(hearts[health]);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<WaterObj>())
        {
            Debug.Log("aa");
            water = other.gameObject;
        }
    }

    public void SetCheckpoint(Vector3 point)
    {
        Debug.Log("checkpoint");
        checkpoint = point;
    }

    private void WaterRespawn()
    {
        
        Debug.Log("on water");
        if (transform.position.y < water.transform.position.y - 5)
        {

            StartCoroutine(Respawn());
        }
        else if(transform.position.y > water.transform.position.y + 2)
        {
            water = null;
            Debug.Log("over water");
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2);
        transform.position = checkpoint;

    }
}
