using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private FSM fsm;
    private bool detected = false;
    private bool hit = false;
    private Animator animator;
    public float health = 1;
    private Character character;
    public Element lowElement;
    
    
    // Start is called before the first frame update
    void Awake()
    {       
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
        attack.AddTransition(isDead, dead);
        //attack.AddTransition(nearTo, attack);
        attack.AddTransition(away, movment);

        fsm = new FSM(idle);
        fsm.StartFSM();
    }

    private void Start()
    {
        character = FindObjectOfType<Character>();
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
        animator.SetTrigger("Walk");
    }

    private void ResetMoveAnim()
    {
        animator.ResetTrigger("Walk");
    }

    private void StartCombatAnim()
    {
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
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Die"));
        animator.ResetTrigger("Hit");
        yield return new WaitForSeconds(2);
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
        if (detected && distanceToCharacter < distanceForAttack)
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



    private bool SeeEnemy()
    {
        if(detected)
            return true;

        return false;
    }

    private bool Idle()
    {
        return !SeeEnemy();
    }

    public float viewAngle = 60f;       // Field of view angle in degrees.
    public float viewRadius = 10f;      // Radius of the view cone.
    public LayerMask targetMask;        // Layer mask to filter the targets.
    //public LayerMask obstacleMask;      // Layer mask for obstacles.
    private float distanceToCharacter;
    public float distanceForAttack;
    public float rotationSpeed = 5f;
    public float speed = 5f;
    public Vector3[] idlePositions;
    private int i = 0;

    bool isGrounded = false;
    /*public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;*/

    private void Update()
    {
        // Check if the character is within the view cone.
        if (CanSeeCharacter())
        {
            detected = true;
        }else
            detected = false;

        fsm.UpdateFSM();
        //Debug.Log(fsm.currentState.name);
    }

    private void Chase()
    {
        //isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        /*if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }*/

        RotateTowardsCharacter();

        Vector3 moveDirection = (character.transform.position - transform.position).normalized;


        Vector3 move = moveDirection * speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, character.transform.position, move.magnitude);
    }

    private void Move()
    {
        //isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        /*if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }*/

        if (Vector3.Distance(transform.position, idlePositions[i]) > 1)
        {
            RotateCharacter(idlePositions[i]);

            Vector3 moveDirection = (idlePositions[i] - transform.position).normalized;


            Vector3 move = moveDirection * speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, idlePositions[i], move.magnitude);
        }
        else
            i+=1;

        if (i == idlePositions.Length)
            i = 0;
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
        if (angleToCharacter < viewAngle / 2f)
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
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            if (lowElement == collision.collider.gameObject.GetComponent<MagicAttack>().element)
            {
                hit = true;
                health -= 1;
                Destroy(collision.gameObject);
            }else
                Destroy(collision.gameObject);

        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.layer == LayerMask.NameToLayer("Bullet"))
        {
            if (lowElement == other.GetComponent<MagicAttack>().element)
            {
                hit = true;
                health -= 1;
                Debug.Log(other.name);
            }
            
                

        }
    }
}
