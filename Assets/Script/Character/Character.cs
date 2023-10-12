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
    public Enemy enemyTarget;
    private bool attacking = false;
    private Canvas canvas;
    private float space = 60;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        movment = GetComponent<Movment>();
        hearts = new GameObject[health];
        
        canvas = FindObjectOfType<Canvas>();
        Reset();
    }

    private void Reset()
    {
        var h = Resources.Load<GameObject>("life");
        health = 3;
        for (int i = 0; i < health; i++)
        {
            hearts[i] = Instantiate(h, new Vector3(30 + space * i, Screen.height - 30, 0), Quaternion.identity, canvas.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            StartCoroutine(Dead());
            
        }
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
        animator.Play("Idle_Battle_SwordAndShield");
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

    public void Attack()
    {
        
        if (activeElement && enemyTarget && !attacking)
        {
            StartCoroutine(AttackRoutine());
        }
        
    }

    private IEnumerator AttackRoutine()
    {
        attacking = true;
        yield return new WaitForSeconds(1);
        if (activeElement && enemyTarget)
        {
            Vector3 velocityBullet = enemyTarget.transform.position - transform.position;
            var b = Instantiate(actualElement.BaseAttack, transform.position + new Vector3(0, 3, 0), Quaternion.identity, transform);
            b.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            Quaternion desiredRotation = Quaternion.LookRotation(velocityBullet.normalized, Vector3.up) * Quaternion.Euler(90, 0, 0);
            b.transform.localRotation = desiredRotation;

            b.GetComponent<Rigidbody>().velocity = velocityBullet.normalized * 10;
            
        }
        attacking = false;
    }

    public void MoveCharacter(Vector3 pos, Vector3 move)
    {
        if(onMoveableSurface)
            transform.position = Vector3.MoveTowards(transform.position, pos, move.magnitude);
    }

    private bool onMoveableSurface = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.GetComponent<Air>())
        {
            onMoveableSurface = true;
        }
        else
            onMoveableSurface = false;

        if (collision.collider.gameObject.GetComponentInParent<Enemy>() && health > 0)
        {
            health -= 1;
            Destroy(hearts[health]);
        }   
    }
}
