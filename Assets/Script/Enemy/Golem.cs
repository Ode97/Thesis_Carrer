using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Awake()
    {
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
        idle.AddTransition(active, movment);
        //idle.AddTransition(getHit, idle);
        //idle.AddTransition(isDead, dead);
        idle.AddTransition(hitted, stunned);

        movment.AddTransition(inactive, idle);
        movment.AddTransition(nearTo, attack);
        //movment.AddTransition(getHit, movment);
        //movment.AddTransition(isDead, dead);
        movment.AddTransition(hitted, stunned);
        //movment.AddTransition(away, movment);

        //attack.AddTransition(getHit, attack);
        //attack.AddTransition(isDead, dead);
        //attack.AddTransition(nearTo, attack);
        attack.AddTransition(away, movment);
        attack.AddTransition(hitted, stunned);

        stunned.AddTransition(active, movment);
        stunned.AddTransition(isDead, dead);

        fsm = new FSM(idle);
        fsm.StartFSM();
    }

    private void Start()
    {
        character = FindObjectOfType<Character>();
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
        animator.SetTrigger("stunned");
    }

    private void ResetStunnedAnim()
    {
        animator.ResetTrigger("stunned");
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
            return true;

        return false;
    }


    private bool IsNear()
    {
        Debug.Log(distanceToCharacter + " " + distanceForAttack);
        if (distanceToCharacter < distanceForAttack)
        {
            return true;
        }

        return false;
    }

    private bool IsAway()
    {
        return !IsNear();
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
        
        if (active)
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



    private void Update()
    {
        Debug.Log(fsm.currentState.name);
        distanceToCharacter = Vector3.Distance(transform.position, character.transform.position);
        if (active && !wait)
        {
            StartCoroutine(WaitInactive(10));
        } else if (!active && !wait)
        { 
            StartCoroutine(WaitActive(5));
        }
        fsm.UpdateFSM();
        //Debug.Log(fsm.currentState.name);
    }

    private IEnumerator WaitActive(int t)
    {
        wait = true;
        yield return new WaitForSeconds(t);
        wait = false;
        active = true;
        Debug.Log("NowIsActive");
    }

    private IEnumerator WaitInactive(int t)
    {
        wait = true;
        yield return new WaitForSeconds(t);
        wait = false;
        active = false;
        Debug.Log("NowIsInactive");
    }

    private void Chase()
    {
        RotateTowardsCharacter();

        Vector3 moveDirection = (character.transform.position - transform.position).normalized;

        Vector3 move = moveDirection * speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, character.transform.position, move.magnitude);
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

        if (other.layer == LayerMask.NameToLayer("Bullet"))
        {
            Debug.Log(other.name);
            if (lowElement == other.GetComponent<MagicAttack>().element)
            {
                hit = true;
                health -= 1;
                Debug.Log("particle" + name);
            }
        }
    }
}
