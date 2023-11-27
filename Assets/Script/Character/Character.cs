using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
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
    private GameObject enemyTarget;
    private bool attacking = false;
    [SerializeField]
    private Canvas lifeCanvas;
    [SerializeField]
    private GameObject lives;
    private float space = 110;
    private GameObject water;
    private Rigidbody rb;
    private int diamond = 0;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        movment = GetComponent<Movment>();
        hearts = new GameObject[health];
        rb = GetComponent<Rigidbody>();
        
        Reset();
    }

    private void Reset()
    {

        var h = Resources.Load<GameObject>("life");
        health = 3;


        for (int i = 0; i < health; i++)
        {
            GameObject heart = Instantiate(h, Vector3.zero, Quaternion.identity);

            // Posiziona il Canvas sopra la testa del personaggio
            lifeCanvas.transform.SetParent(transform);
            
            lifeCanvas.transform.localPosition = Vector3.up * 3; // Puoi regolare questa posizione a seconda delle tue esigenze

            // Aggiungi il cuore all'array hearts
            hearts[i] = heart;

            heart.transform.SetParent(lives.transform);
                      
            // Scala del cuore
            heart.transform.localScale = new Vector3(0, 0, 0);
            heart.SetActive(false);
        }
    }

    private bool heartAnim = false;

    /*public void Life()
    {
        for (int i = 0; i < health; i++)
        {
            hearts[i].transform.LookAt(Camera.main.transform);
            hearts[i].SetActive(true);
            heartAnim = true;
        }
    }*/

    public void Life()
    {
        // Assicurati che hearts non sia nullo e health sia maggiore di zero

        // Calcola il numero di cuori attivi
        if (health > 0)
        {
            int activeHearts = health;

            // Ottieni le dimensioni del canvas
            RectTransform canvasRect = hearts[0].transform.parent.GetComponent<RectTransform>();
            Vector2 canvasSize = canvasRect.sizeDelta;

            // Calcola lo spazio tra i cuori
            float spacing = 100f; // Puoi regolare lo spaziamento come preferisci

            // Calcola la larghezza totale dei cuori e dello spaziamento
            float totalWidth = (activeHearts - 1) * spacing;

            // Calcola la posizione iniziale per centrare i cuori
            float startX = 0 - totalWidth / 2;

            // Posiziona i cuori
            for (int i = 0; i < activeHearts; i++)
            {
                Vector3 heartPosition = new Vector3(startX + i * spacing, canvasSize.y / 2f, 0f);
                hearts[i].transform.localPosition = heartPosition;
                hearts[i].transform.LookAt(Camera.main.transform);
                hearts[i].SetActive(true);
                heartAnim = true;
            }
        }
    }

    private IEnumerator DisableLife()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < health; i++)
        {
            heartAnim = false;
        }
    }

    private bool dead = false;
    // Update is called once per frame
    void Update()
    {
        if (health > 0)
        {
            if (heartAnim && hearts[0].transform.localScale != Vector3.one)
            {
                for (int i = 0; i < health; i++)
                {
                    hearts[i].transform.localScale = Vector3.Lerp(hearts[i].transform.localScale, Vector3.one, Time.deltaTime * 5);
                }
            }
            else if (hearts[0].transform.localScale != Vector3.zero)
            {
                for (int i = 0; i < health; i++)
                {
                    hearts[i].transform.localScale = Vector3.Lerp(hearts[i].transform.localScale, Vector3.zero, Time.deltaTime * 5);
                }
            }
        }

        if (water)
        {
            WaterRespawn();
        }

        if(health <= 0 && !dead)
        {
            StartCoroutine(Dead());
            
        }

        if (enemyTarget)
        {
            Attack();
        }
        else
            if (activeAttack)
            Destroy(activeAttack);
    }

    private IEnumerator Dead()
    {
        dead = true;
        GameManager.instance.stopLogic = true;
        animator.SetTrigger("Die");
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Die01_Stay_SwordAndShield"));
        animator.ResetTrigger("Die");
        yield return new WaitForSeconds(3);
        Reset();
        StartCoroutine(Respawn());
        animator.Play("Idle_Battle_SwordAndShield");
        dead = false;
    }

    public void SetEnemy(GameObject e)
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

        if(activeAttack) 
            Destroy(activeAttack);
    }

    public bool IsMoving()
    {
        return movment.IsWalking() || movingPlt;
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
            enemyTarget = null;
            return;
        }

        

        activeElement = true;

        aurea = Instantiate(element.aurea, transform);
        actualElement = element;
    }

    public void DisableElement()
    {
        Destroy(aurea.gameObject);
        actualElement = null;
        activeElement = false;
        return;
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

            activeAttack = Instantiate(actualElement.BaseAttack);
            

            if (activeAttack.GetComponent<MagicAttack>().element != Element.Earth)
                activeAttack.transform.position = transform.position + new Vector3(0, 3, 0) + transform.forward * 3;
            else
                activeAttack.transform.position = transform.position + transform.forward * 3;

            Vector3 enemyPos = enemyTarget.transform.position;
            Vector3 target = new Vector3(enemyPos.x, activeAttack.transform.position.y, enemyPos.z);
            activeAttack.transform.LookAt(target);
        }
        else if(activeAttack)
        {
            //activeAttack.transform.rotation = Quaternion.identity;
            var attackElement = activeAttack.GetComponent<MagicAttack>().element;
            if (attackElement != Element.Air)
            {
                if (attackElement != Element.Earth)
                    activeAttack.transform.position = transform.position + new Vector3(0, 3, 0) + transform.forward * 3;
                else
                    activeAttack.transform.position = transform.position + transform.forward * 3;
            }

            Vector3 enemyPos = enemyTarget.transform.position;
            Vector3 target = new Vector3 (enemyPos.x, activeAttack.transform.position.y, enemyPos.z);
            activeAttack.transform.LookAt(target);
            transform.LookAt(enemyTarget.transform);
            
            /*if(activeAttack.GetComponent<MagicAttack>().element == Element.Air)
            {
                activeAttack.transform.position = Vector3.Lerp(activeAttack.transform.position, target, Time.deltaTime * bulletVelocity);
            }*/
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

        if(collision.gameObject.layer == Constants.bulletLayer)
        {
            health -= 1;

            Life();

            Destroy(hearts[health]);
            var rb = collision.transform.GetComponent<Rigidbody>();
            Debug.Log("aaa");
            if (!rb.isKinematic) 
            {
                /*collision.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
                collision.transform.GetComponent<Rigidbody>().isKinematic = true;
                collision.transform.SetParent(transform);
                StartCoroutine(DestroyBullet(collision.gameObject));*/
                Destroy(collision.gameObject);
            }
        }

        if (collision.gameObject.layer == Constants.enemyLayer && health > 0)
        {
            Debug.Log("colpito");
            health -= 1;
            
            Life();
            
            Destroy(hearts[health]);
        }else if(collision.gameObject.layer == Constants.diamondLayer)
        {
            diamond++;
            Destroy(collision.gameObject);
        }
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<WaterObj>())
        {
            water = other.gameObject;
        }
    }

    public void SetCheckpoint(Vector3 point)
    {
        Debug.Log("checkpoint");
        checkpoint = point;
    }

    private bool movingPlt = true;
    public void AddForce(Vector3 force)
    {
        rb.AddForce(force, ForceMode.Force);
        movingPlt = true;
    }
    public void StopPlt()
    {
        movingPlt = false;
    }

    private void WaterRespawn()
    {

        if (Vector3.Distance(water.transform.position, transform.position) > 80)
        {
            Debug.Log(Vector3.Distance(water.transform.position, transform.position));
            water = null;
            return;
        }

        if (transform.position.y < water.transform.position.y - 5)
        {
            Debug.Log("water");
            StartCoroutine(Respawn());
        }
        else if(transform.position.y > water.transform.position.y + 2)
        {
            water = null;
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2);
        movment.DisableMove();
        transform.position = checkpoint;
        GameManager.instance.stopLogic = false;
    }

    public void OnMouseEnter()
    {
        Life();
    }

    public void OnMouseExit()
    {
        StartCoroutine(DisableLife());
    }
}
