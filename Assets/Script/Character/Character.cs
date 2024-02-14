using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

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
    private GameObject gems;
    private TextMeshProUGUI textDiams;
    [SerializeField]
    private GameObject lives;
    private float space = 110;
    //private GameObject water;
    private Rigidbody rb;
    private int diamond = 0;
    private bool moving = false;
    private Vector3 initPosition;
    private Quaternion initRotation;
    private bool gameStart = false;

    //avoid to trigger checkpoint save before the start of the game
    public void GameStart()
    {
        gameStart = true;
    }

    public bool IsGameStart()
    {
        return gameStart;
    }

    public bool IsAttacking()
    {
        return attacking;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        movment = GetComponent<Movment>();
        
        rb = GetComponent<Rigidbody>();
        lastPos = transform.position;
        initPosition = transform.position;
        initRotation = transform.rotation;
        textDiams = gems.GetComponentInChildren<TextMeshProUGUI>();
        hearts = new GameObject[health];
        lifeCanvas.transform.parent.SetParent(transform);
        lifeCanvas.transform.parent.transform.localPosition = Vector3.zero;
        EventManager.StartListening("Reset", Reset);
        ResetLife();                
    }

    public void NewGame()
    {
        transform.position = initPosition;
        transform.rotation = initRotation;
        diamond = 0;
        checkpoint = null;
    }

    private void Reset()
    {
        gameStart = false;
        ResetLife();
    }

    public void SetHealth(int h)
    {
        health = h;
    }

    private void ResetLife()
    {

        var h = Resources.Load<GameObject>("life");
        health = 3;


        for (int i = 0; i < health; i++)
        {
            GameObject heart = Instantiate(h, Vector3.zero, Quaternion.identity);

            // Posiziona il Canvas sopra la testa del personaggio
            //lifeCanvas.transform.SetParent(transform);
            
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
            float spacing = 100f;

            // Calcola la larghezza totale dei cuori e dello spaziamento
            float totalWidth = (activeHearts - 1) * spacing;

            // Calcola la posizione iniziale per centrare i cuori
            float startX = 0 - totalWidth / 2;

            lifeCanvas.transform.rotation = transform.rotation;
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
        textDiams.text = diamond.ToString();
        gems.gameObject.SetActive(true);
        
    }

    private IEnumerator DisableLife()
    {
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < health; i++)
        {
            heartAnim = false;
        }
        gems.gameObject.SetActive(false);
    }

    private bool dead = false;
    private Vector3 lastPos;
    // Update is called once per frame
    void Update()
    {
        if (lastPos != transform.position)
        {
            moving = true;
        }
        else
            moving = false;

        lastPos = transform.position;

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


        if(health <= 0 && !dead)
        {
            StartCoroutine(Dead());
            
        }

        if (enemyTarget)
        {
            child = enemyTarget;
            while (child.transform.parent != null)
            {
                child = child.transform.parent.gameObject;
            }            
            
            if (!child.activeSelf)
            {
                enemyTarget = null;
                Destroy(activeAttack);           
            }
            else
                Attack();
        }
        
        
        
    }

    private GameObject child;

    public bool IsDead()
    {
        return dead;
    }

    private IEnumerator Dead()
    {
        dead = true;
        GameManager.instance.stopLogic = true;
        animator.SetTrigger("Die");
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Die01_Stay_SwordAndShield"));
        animator.ResetTrigger("Die");
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(Respawn());
        ResetLife();
        animator.Play("Idle_Battle_SwordAndShield");
        AudioManager.instance.PlayForestMusic();
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
            DisableElement();
        }

        if (activeAttack)        
            Destroy(activeAttack);
        
    }

    public bool IsMoving()
    {
        return moving;
    }

    public void MoveToDestination(Vector3 point)
    {
        movment.Move(point);
    }

    public void SetActualElement(MagicElement element)
    {
        if (!GameManager.instance.airEffect)
        {
            if (aurea != null)
                Destroy(aurea.gameObject);


            if (activeAttack)
            {

                Destroy(activeAttack);
            }

            if (activeElement && element == actualElement)
            {
                //actualElement = null;
                activeElement = false;
                enemyTarget = null;
                return;
            }



            activeElement = true;

            aurea = Instantiate(element.aurea, transform);
            actualElement = element;
        }
    }

    public void DisableElement()
    {
        Destroy(aurea.gameObject);
        //actualElement = null;
        activeElement = false;
        enemyTarget = null;
        return;
    }

    public bool IsActiveElement()
    {
        return activeElement;
    }

    public Element GetActualElement()
    {
        if (activeElement)
            return actualElement.element;
        else
            return Element.None;
    }

    public MagicElement GetMagicElement()
    {
        return actualElement;
    }

    public void SetObject(InteractableObject obj)
    {
        this.obj = obj;
    }

    public InteractableObject GetObject()
    {
        return obj;
    }

    public void Interaction(RaycastHit hit)
    {

        if (activeElement)
        {
            Debug.Log(obj.name + " " + obj.gameObject.activeSelf);
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
            
            activeAttack.GetComponent<MagicAttack>().AudioCast();

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



    public int GetDiamonds()
    {
        return diamond;
    }

    public void SetDiamonds(int d)
    {
        diamond = d;
    }

    private Checkpoint checkpoint;
    private GameObject airObj;
    [SerializeField]
    private AudioSource hitted;
    private void OnCollisionEnter(Collision collision)
    {

        if(collision.gameObject.layer == Constants.bulletLayer && health > 0)
        {
            health -= 1;

            Life();

            Destroy(hearts[health]);
            var rb = collision.transform.GetComponent<Rigidbody>();
            
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
            hitted.Play();
            health -= 1;
            
            Life();
            
            Destroy(hearts[health]);
        }else if(collision.gameObject.layer == Constants.diamondLayer)
        {
            diamond++;
            
            collision.gameObject.GetComponent<AudioSource>().Play();
            collision.gameObject.GetComponent<MeshRenderer>().enabled = false;
            collision.collider.enabled = false;
            GameManager.instance.Save();
            //collision.gameObject.SetActive(false);

        }
    }

    public void SetPlatform(GameObject g)
    {
        airObj = g;
        platform = true;
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<WaterObj>())
        {
            water = other.gameObject;
        }
    }*/

    public void SetCheckpoint(Checkpoint cP)
    {
        checkpoint = cP;
        Debug.Log(checkpoint.GetIndex());
        platform = false;
    }

    public int GetCheckpointIndex()
    {
        var x = 0;
        if (checkpoint)
            x = checkpoint.GetIndex();
        return x;
    }

    /*private void WaterRespawn()
    {

        

        if (transform.position.y < water.transform.position.y - 5)
        {
            
            StartCoroutine(Respawn());
        }
        

        if (Vector3.Distance(water.transform.position, transform.position) > 80)
        {
            Debug.Log(Vector3.Distance(water.transform.position, transform.position));
            water = null;
            return;
        }

        if (transform.position.y > water.transform.position.y + 2)
        {
            water = null;
        }
    }*/

    private float waterTimer = 0;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<WaterObj>())
        {
            if (transform.position.y < other.transform.position.y - 5)
            {
                waterTimer += Time.deltaTime;
                //water = other.gameObject;
                if (waterTimer > 2)
                {
                    waterTimer = 0;
                    StartCoroutine(Respawn());
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<WaterObj>())
        {
            waterTimer = 0;
        }
    }

    private bool platform = false;
    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1);
        
        movment.DisableMove();

        if(platform)
            transform.position = airObj.transform.position;
        else
            transform.position = checkpoint.transform.position;

        Debug.Log(checkpoint.GetIndex());
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
