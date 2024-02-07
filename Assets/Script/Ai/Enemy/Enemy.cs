using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private FSM fsm;
    private bool detected = false;
    private bool hit = false;
    private Animator animator;
    public int health = 1;
    private Character character;
    public Element lowElement;
    private NavMeshAgent agent;
    private Vector3 initPos;
    private Quaternion initRot;
    private bool enigma;


    // Start is called before the first frame update
    void Awake()
    {
        initPos = transform.localPosition;
        initRot = transform.localRotation;
        animator = GetComponent<Animator>();

        FSMState idle = new FSMState("idle");
        FSMState movment = new FSMState("movment");
        FSMState attack = new FSMState("attack");
        FSMState dead = new FSMState("dead");

        idle.enterActions.Add(MoveAnim);
        idle.enterActions.Add(ResetHitAnim);
        idle.exitActions.Add(ResetMoveAnim);
        idle.stayActions.Add(Move);

        movment.enterActions.Add(StartCombatAnim);
        movment.enterActions.Add(ResetHitAnim);
        movment.exitActions.Add(ResetStartCombatAnim);
        movment.stayActions.Add(Chase);

        attack.enterActions.Add(AttackAnim);
        attack.enterActions.Add(ResetHitAnim);
        attack.exitActions.Add(ResetAttackAnim);
        attack.stayActions.Add(RotateTowardsCharacter);
        dead.enterActions.Add(DeadAnim);

        FSMTransition outOfView = new FSMTransition(Idle);
        FSMTransition characterDetection = new FSMTransition(SeeEnemy);        
        FSMTransition nearTo = new FSMTransition(IsNear);
        FSMTransition away = new FSMTransition(IsAway);
        FSMTransition isDead = new FSMTransition(Dead);
        FSMAction[] a = new FSMAction[1];
        a[0] = HitAnim;
        FSMTransition getHit = new FSMTransition(GetHit, a);

        //idle.AddTransition(outOfView, idle);
        idle.AddTransition(characterDetection, movment);        
        idle.AddTransition(getHit, idle);
        idle.AddTransition(isDead, dead);

        movment.AddTransition(outOfView, idle);
        movment.AddTransition(nearTo, attack);        
        movment.AddTransition(getHit, movment);
        movment.AddTransition(isDead, dead);

        //movment.AddTransition(away, movment);

        attack.AddTransition(getHit, attack);       
        //attack.AddTransition(nearTo, attack);
        attack.AddTransition(away, movment);
        attack.AddTransition(isDead, dead);

        fsm = new FSM(idle);

        character = FindObjectOfType<Character>();
        agent = GetComponent<NavMeshAgent>();

        if (idlePositions.Length > 0)
        {
            agent.isStopped = false;
            agent.SetDestination(idlePositions[0]);
        }else
            agent.isStopped = false;


        
        fsm.StartFSM();
    }

    private void Start()
    {

        EventManager.StartListening("Reset", Reset);
        

    }

    public void SetEnigma()
    {
        enigma = true;
    }

    private void HitAnim()
    {
        animator.SetTrigger("Hit");
        hit = false;
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

    private void MoveAnim()
    {
        //Debug.Log("walk");
        
        //animator.SetTrigger("Walk");
    }

    private void ResetMoveAnim()
    {
        animator.ResetTrigger("Walk");
    }

    private void StartCombatAnim()
    {
        Debug.Log("combat e goto" + gameObject.name);
        animator.SetTrigger("Combat");
        animator.SetTrigger("GoTo");
    }

    private void ResetStartCombatAnim()
    {
        animator.ResetTrigger("Combat");
        animator.ResetTrigger("GoTo");
    }

    private void DeadAnim()
    {
        animator.SetTrigger("Dead");
        
        StartCoroutine(DestroyAfterDeath());
    }

    private IEnumerator DestroyAfterDeath()
    {
        
        //Destroy(collider);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Die"));
        animator.ResetTrigger("Hit");
        agent.isStopped = true;
        
        yield return new WaitForSeconds(0.5f);
        if (enigma)
            transform.parent.GetComponentInParent<Enigma>().ActiveAllCheck();
        gameObject.SetActive(false);
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
        //Debug.Log("getHit");
        if (hit)
            return true;

        return false;
    }


    private bool IsNear()
    {
        //Debug.Log("isNear");
        
        if (detected && distanceToCharacter < distanceForAttack)
        {
            return true;
        }

        return false;
    }

    private bool IsAway()
    {
        //Debug.Log("isAway");
        //Debug.Log("is away: " + !IsNear());
        return !IsNear();
    }

    private bool Dead()
    {

        //Debug.Log("Dead");
        if (health <= 0)
        {
            return true;
        }

        return false;
    }



    private bool SeeEnemy()
    {
        //Debug.Log("SeeEnemy");
        if (detected)
            return true;

        return false;
    }

    private bool Idle()
    {
        //Debug.Log("Idle");
        return !SeeEnemy();
    }

    public float viewAngle = 60f;       // Field of view angle in degrees.
    public float viewRadius = 10f;      // Radius of the view cone.
    public LayerMask targetMask;        // Layer mask to filter the targets.
    //public LayerMask obstacleMask;      // Layer mask for obstacles.
    private float distanceToCharacter;
    public float distanceForAttack;
    public float rotationSpeed = 5f;
    //public float speed = 5f;
    public Vector3[] idlePositions;
    private int i = 0;

    bool isGrounded = false;
    /*public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;*/

    private void Update()
    {
        if (!MenuManager.instance.isMenuOpen())
        {
            // Check if the character is within the view cone.
            if (CanSeeCharacter())
            {
                detected = true;
            }
            else
                detected = false;


            fsm.UpdateFSM();
            
        }else
            agent.isStopped = true;
        
        //Debug.Log(gameObject.name + " " + fsm.currentState.name);
    }

    private void Chase()
    {
        //isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        /*if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }*/

        /*RotateTowardsCharacter();

        Vector3 moveDirection = (character.transform.position - transform.position).normalized;


        Vector3 move = moveDirection * speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, character.transform.position, move.magnitude);*/

        agent.isStopped = false;
        agent.SetDestination(character.transform.position);
    }

    private void Move()
    {
        //isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        /*if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }*/

        if (idlePositions.Length > 1)
        {
            agent.isStopped = false;
            if (Vector3.Distance(transform.position, idlePositions[i]) < 1f)
            {
                i += 1;
                if (i >= idlePositions.Length)
                    i = 0;
                /*RotateCharacter(idlePositions[i]);

                Vector3 moveDirection = (idlePositions[i] - transform.position).normalized;


                Vector3 move = moveDirection * speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, idlePositions[i], move.magnitude);*/
                //Debug.Log(Vector3.Distance(transform.position, idlePositions[i]));


                agent.SetDestination(idlePositions[i]);
            }
        }
       
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

    private bool CanSeeCharacter()
    {
        Vector3 directionToCharacter = character.transform.position - transform.position;
        float angleToCharacter = Vector3.Angle(transform.forward, directionToCharacter);

        // Check if the character is within the view angle and view radius.
        if (angleToCharacter < viewAngle / 2f && !character.IsDead())
        {
            distanceToCharacter = Vector3.Distance(transform.position, character.transform.position);
            //Debug.Log(Physics.Raycast(transform.position, directionToCharacter, distanceToCharacter, LayerMask.NameToLayer("Terrain") | LayerMask.NameToLayer("InteractableObject")));

            bool obstacle = Physics.Raycast(transform.position, directionToCharacter, distanceToCharacter, LayerMask.NameToLayer("Terrain") | LayerMask.NameToLayer("InteractableObject"));
            // Check if there are no obstacles between the monster and the character.
            if (!obstacle && distanceToCharacter < viewRadius)
            {
                return true; // Character is visible within the cone.
            }
        }

        return false;
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Handles.color = Color.yellow;

        Vector3 viewDirA = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward;
        Vector3 viewDirB = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward;

        Gizmos.DrawRay(transform.position, viewDirA * viewRadius);
        Gizmos.DrawRay(transform.position, viewDirB * viewRadius);

        //float arcRadius = viewRadius * Mathf.Sin(viewAngle * 0.5f * Mathf.Deg2Rad);
        Handles.DrawWireArc(transform.position, Vector3.up, viewDirA, viewAngle, viewRadius);
        

        if (character != null)
        {
            Vector3 directionToCharacter = character.transform.position - transform.position;
            Gizmos.color = CanSeeCharacter() ? Color.red : Color.yellow;
            Handles.color = CanSeeCharacter() ? Color.red : Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + directionToCharacter.normalized * viewRadius);
        }
    }
#endif

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            if (lowElement == collision.collider.gameObject.GetComponent<MagicAttack>().element)
            {
                hit = true;
                health -= 1;
                Destroy(collision.gameObject);
            }else
                Destroy(collision.gameObject);

            Debug.Log("collision");
        }
    }

    private bool audio = false;
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.gameObject.name);
        if (other.layer == LayerMask.NameToLayer("Bullet"))
        {
            WaterBall w;
            if (other.TryGetComponent<WaterBall>(out w))
                w.DestroyBall();

            var attk = other.GetComponent<MagicAttack>();

            if (!audio)
            {
                attk.AudioImpact();
                audio = true;
            }

            if (!attk.IsImpactPlaying())
            {
                audio = false;
            }

            if (lowElement == attk.element && health > 0)
            {
                hit = true;
                health -= 1;
                if (health == 0)
                    agent.isStopped = true;
                //Debug.Log(health);
                //Debug.Log("particle" + name);
            }
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public void SetHealth(int h)
    {
        health = h;
        if(h <= 0)
            gameObject.SetActive(false);
    }

    private void Reset()
    {
        transform.localPosition = initPos;
        transform.localRotation = initRot;
        health = 1;
        gameObject.SetActive(true);
    }


    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            if (lowElement == other.GetComponent<MagicAttack>().element)
            {
                hit = true;
                health -= 1;
                Debug.Log("particle 2");
            }
        }
    }*/


}
