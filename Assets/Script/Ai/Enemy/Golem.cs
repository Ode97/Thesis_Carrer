using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Golem : MonoBehaviour
{
    private FSM fsm;
    private bool active = false;
    private bool hit = false;
    private bool wait = false;
    private Animator animator;
    public int health = 1;
    private Character character;
    public Element lowElement;
    private GameObject aurea;
    private ParticleSystem aureaPS;
    Material mat;
    private NavMeshAgent agent;
    [SerializeField]
    private GameObject sphere;
    private Vector3 sphereInitPos;
    private Vector3 initPos;

    void Awake()
    {
        /*aurea = Instantiate(Resources.Load<GameObject>("GolemEffect"), transform);
        aurea.transform.localPosition = Vector3.zero;
        aurea.transform.localScale = new Vector3(4,4,4);
        aureaPS = aurea.GetComponent<ParticleSystem>();*/
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;

        animator = GetComponent<Animator>();

        FSMState idle = new FSMState("idle");
        FSMState movment = new FSMState("movment");
        FSMState attack = new FSMState("attack");
        FSMState dead = new FSMState("dead");
        FSMState stunned = new FSMState("stunned");

        idle.enterActions.Add(IdleAnim);
        idle.exitActions.Add(ResetIdleAnim);
        //idle.enterActions.Add(ResetHitAnim);
        //idle.exitActions.Add(ResetMoveAnim);
        //idle.stayActions.Add(Move);

        movment.enterActions.Add(ChaseAnim);
        //movment.enterActions.Add(ResetHitAnim);
        movment.exitActions.Add(ResetChaseAnim);
        movment.stayActions.Add(Chase);

        attack.enterActions.Add(AttackAnim);
        //attack.enterActions.Add(ResetHitAnim);
        attack.exitActions.Add(ResetAttackAnim);
        attack.stayActions.Add(RotateTowardsCharacter);

        dead.enterActions.Add(DeadAnim);

        stunned.enterActions.Add(StunnedAnim);
        stunned.exitActions.Add(ResetStunnedAnim);

        //FSMTransition outOfView = new FSMTransition(Idle);
        //FSMTransition characterDetection = new FSMTransition(SeeEnemy);
        FSMTransition nearTo = new FSMTransition(IsNear);
        FSMTransition away = new FSMTransition(IsAway);
        FSMTransition hitted = new FSMTransition(GetHit);
        FSMTransition isDead = new FSMTransition(Dead);
        FSMTransition active = new FSMTransition(IsActive);
        FSMTransition inactive = new FSMTransition(IsInactive);
        //FSMAction[] a = new FSMAction[1];
        //a[0] = HitAnim;
        //FSMTransition getHit = new FSMTransition(GetHit, a);

        //idle.AddTransition(outOfView, idle);
        idle.AddTransition(hitted, stunned);
        idle.AddTransition(active, movment);
        //idle.AddTransition(getHit, idle);
        //idle.AddTransition(isDead, dead);

        movment.AddTransition(hitted, stunned);
        movment.AddTransition(inactive, idle);
        movment.AddTransition(nearTo, attack);
        //movment.AddTransition(getHit, movment);
        //movment.AddTransition(isDead, dead);
        //movment.AddTransition(away, movment);

        //attack.AddTransition(getHit, attack);
        //attack.AddTransition(isDead, dead);
        //attack.AddTransition(nearTo, attack);

        attack.AddTransition(hitted, stunned);
        attack.AddTransition(away, movment);
        

        stunned.AddTransition(isDead, dead);
        stunned.AddTransition(active, movment);
        

        fsm = new FSM(idle);
        
    }

    private void Start()
    {
        health = 2;
        hearts = new GameObject[health];
        lifeCanvas.transform.parent.SetParent(transform);
        lifeCanvas.transform.parent.transform.localPosition = Vector3.zero;
        Reset();

        character = FindObjectOfType<Character>();
        agent = GetComponent<NavMeshAgent>();
        target = character.transform.position;
        sphereInitPos = sphere.transform.localPosition;
        initPos = transform.localPosition;
        ChangeElement();
    }


    private void IdleAnim()
    {
        
        animator.SetTrigger("idle");
        
    }

    private void ResetIdleAnim()
    {
        animator.ResetTrigger("idle");
    }

    private void StunnedAnim()
    {
        animator.SetTrigger("Hit");
        
    }

    private void ResetStunnedAnim()
    {
        animator.ResetTrigger("Hit");
        change = false;
    }

    private void ResetHitAnim()
    {
        StartCoroutine(WaitHitAnim());
    }

    private IEnumerator WaitHitAnim()
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("GetHit"));
        animator.ResetTrigger("Hit");
    }

    private void ChaseAnim()
    {
        
        animator.SetTrigger("Walk");
        
    }

    private void ResetChaseAnim()
    {
        animator.ResetTrigger("Walk");
        
        agent.isStopped = true;
        agent.destination = transform.position;
    }

    /*private void StartCombatAnim()
    {
        animator.SetTrigger("Combat");
        animator.SetTrigger("GoTo");
    }

    private void ResetStartCombatAnim()
    {
        animator.ResetTrigger("Combat");
        animator.ResetTrigger("GoTo");
    }*/

    private void DeadAnim()
    {
        animator.SetTrigger("Dead");
        StartCoroutine(DestroyAfterDeath());
    }

    private IEnumerator DestroyAfterDeath()
    {
        //Destroy(collider);
        AudioManager.instance.PlayForestMusic();
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Die"));
        animator.ResetTrigger("Hit");
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    private void ResetDeadAnim()
    {
        animator.ResetTrigger("Dead");
    }

    private void AttackAnim()
    {
        agent.isStopped = true;
        animator.SetTrigger("Attack");
    }

    private void ResetAttackAnim()
    {
        animator.ResetTrigger("Attack");
    }

    private bool GetHit()
    {
        if (hit)
        {
            return true;
        }

        return false;
    }   


    private bool IsNear()
    {
        target = character.transform.position;
        if (distanceToCharacter < distanceForAttack)
        {
            return true;
        }

        return false;
    }

    private bool IsAway()
    {
        return !IsNear() && !hit;
    }

    private bool Dead()
    {
        if (health <= 0)
        {
            return true;
        }

        return false;
    }

    

    private bool IsActive()
    {
        
        if (active && !hit)
            return true;

        return false;
    }

    private bool IsInactive()
    {
        return !IsActive();
    }

    

    //public float viewAngle = 60f;       // Field of view angle in degrees.
    //public float viewRadius = 10f;      // Radius of the view cone.
    //public LayerMask targetMask;        // Layer mask to filter the targets.
    //public LayerMask obstacleMask;      // Layer mask for obstacles.
    private float distanceToCharacter;
    public float distanceForAttack;
    public float rotationSpeed = 5f;
    public float speed = 5f;
    //public Vector3[] idlePositions;
    private int i = 0;
    private bool change = false;
    private bool stop = false;
    private Vector3 target;
    private bool sphereReposition = false;
    private void Update()
    {
        if (!MenuManager.instance.isMenuOpen())
        {

            if (!stop)
                StartCoroutine(Change());

            if (change)
            {
                ChangeElement();
            }

            distanceToCharacter = Vector3.Distance(transform.position, target);
            if (active && !wait)
            {
                StartCoroutine(WaitInactive(10));
            }
            else if (!active && !wait)
            {
                StartCoroutine(WaitActive(10));
            }

            if (sphereReposition)
            {
                Vector3 moveDirection = (sphereInitPos - sphere.transform.localPosition).normalized;

                sphere.transform.localPosition = Vector3.MoveTowards(sphere.transform.localPosition, sphereInitPos, 10 * moveDirection.magnitude * Time.deltaTime);

                Debug.Log(sphere.transform.localPosition);
                if (Vector3.Distance(sphere.transform.localPosition, sphereInitPos) < 1)
                {
                    sphereReposition = false;
                    sphere.GetComponent<Rigidbody>().isKinematic = false;
                }
            }

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

            fsm.UpdateFSM();
            //Debug.Log(fsm.currentState.name);
        }
        else
            agent.isStopped = true;
    }

    private IEnumerator Change()
    {
        stop = true;
        yield return new WaitForSeconds(8);
        elem = Random.Range(0, 4);
        stop = false;
        change = !change;
    }

    //var ps = aureaPS.main;
    private int elem = 0;
    private void ChangeElement()
    {
        

        if (elem == 0)
        {
            if (mat.color == Color.gray)
            {

                return;
            }

            lowElement = Element.Earth;
            //ps.startColor = Color.gray;            
            mat.color = Color.Lerp(GetComponentInChildren<SkinnedMeshRenderer>().material.color, Color.gray, Time.deltaTime * 5);
        }
        else if (elem == 1)
        {
            if (mat.color == Color.green)
            {

                return;
            }
            lowElement = Element.Fire;
            //ps.startColor = Color.green;
            mat.color = Color.Lerp(GetComponentInChildren<SkinnedMeshRenderer>().material.color, Color.green, Time.deltaTime * 5);
        }
        else if (elem == 2)
        {
            if (mat.color == Color.red)
            {

                return;
            }
            lowElement = Element.Water;
            //ps.startColor = Color.red;
            mat.color = Color.Lerp(GetComponentInChildren<SkinnedMeshRenderer>().material.color, Color.red, Time.deltaTime * 5);
        }
        else if (elem == 3)
        {
            if (mat.color == Color.blue)
            {
                return;
            }
            lowElement = Element.Air;
            //ps.startColor = Color.blue;
            mat.color = Color.Lerp(GetComponentInChildren<SkinnedMeshRenderer>().material.color, Color.blue, Time.deltaTime * 5);
        }

    }

    private IEnumerator WaitActive(int t)
    {
        wait = true;
        yield return new WaitForSeconds(t);
        wait = false;
        active = true;
    }

    private IEnumerator WaitInactive(int t)
    {
        wait = true;
        yield return new WaitForSeconds(t);
        wait = false;
        active = false;
    }

    private void Chase()
    {
        //RotateTowardsCharacter();

        /*Vector3 moveDirection = (character.transform.position - transform.position).normalized;

        Vector3 move = moveDirection * speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, character.transform.position, move.magnitude);*/
        agent.isStopped = false;
        if (character.IsDead())
        {
            agent.SetDestination(initPos);
            health = 2;
            
        }
        else
            agent.SetDestination(character.transform.position);

    }

    private void RotateTowardsCharacter()
    {
        RotateCharacter(character.transform.position);
    }

    private void RotateCharacter(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }


    private GameObject[] hearts;
    [SerializeField]
    private Canvas lifeCanvas;
    [SerializeField]
    private GameObject lives;
    private bool heartAnim = false;

    private void Reset()
    {

        var h = Resources.Load<GameObject>("life");
        health = 2;


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
    }

    private IEnumerator DisableLife()
    {
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < health; i++)
        {
            heartAnim = false;
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.layer == LayerMask.NameToLayer("Bullet") && hit)
        {
            if (lowElement == other.GetComponent<MagicAttack>().element)
            {
                hit = false;
                health -= 1;
                sphereReposition = true;
                sphere.GetComponent<Rigidbody>().isKinematic = true;

                Life();

                Destroy(hearts[health]);
            }
        }
    }

    private bool sphereHit = false;
    private IEnumerator EndStun()
    {
        yield return new WaitForSeconds(10);
        hit = false;
        active = true;
        wait = false;
        sphereHit = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.layer == LayerMask.NameToLayer("BossTrigger") && !sphereHit)
        {
            Debug.Log(collision.gameObject.name + " mi ha colpito");
            hit = true;
            sphereHit = true;
            
            agent.SetDestination(sphere.transform.position);
            target = sphere.transform.position;
            StopAllCoroutines();
            StartCoroutine(EndStun());
            
        }else if (sphereHit)
        {
            agent.SetDestination(character.transform.position);
            target = character.transform.position;
            
        }
    }

    public void StartFSM()
    {
        fsm.StartFSM();
    }
}
