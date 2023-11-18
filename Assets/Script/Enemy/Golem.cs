using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Golem : MonoBehaviour
{
    private FSM fsm;
    private bool active = false;
    private bool hit = false;
    private bool wait = false;
    private Animator animator;
    public float health = 1;
    private Character character;
    public Element lowElement;
    private GameObject aurea;
    private ParticleSystem aureaPS;
    Material mat;
    private NavMeshAgent agent;
    [SerializeField]
    private GameObject sphere;

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
        fsm.StartFSM();
    }

    private void Start()
    {
        character = FindObjectOfType<Character>();
        agent = GetComponent<NavMeshAgent>();
        target = character.transform.position;
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

    private void Update()
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
        } else if (!active && !wait)
        { 
            StartCoroutine(WaitActive(10));
        }
        fsm.UpdateFSM();
        //Debug.Log(fsm.currentState.name);
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

    private void OnParticleCollision(GameObject other)
    {
        if (other.layer == LayerMask.NameToLayer("Bullet") && hit)
        {
            if (lowElement == other.GetComponent<MagicAttack>().element)
            {
                hit = false;
                health -= 1;
                Debug.Log("particle" + name);
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
    }

    private void OnCollisionEnter(Collision collision)
    {
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
}
