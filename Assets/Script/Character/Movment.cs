using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movment : MonoBehaviour
{
    private Animator characterAnimator; // Assign your character's animator
    public float rotationSpeed = 5.0f; // Rotation speed of the character

    private bool isWalking = false;   // Flag to check if character is walking
    private Vector3 targetPosition;

    public GameObject targetIndicator;
    private ParticleSystem ps;


    public float speed = 5;
    public float gravity = -9.18f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    private void Start()
    {
        characterAnimator = GetComponent<Animator>();
        ps = targetIndicator.GetComponent<ParticleSystem>();
        ps.Pause();
        targetIndicator.SetActive(false);
    }

    private void Update()
    {
        if (targetPosition != null)
        {
            Vector2 character = new Vector2(transform.position.x, transform.position.z);
            Vector2 targetPos = new Vector2(targetPosition.x, targetPosition.z);
            if (Vector2.Distance(character, targetPos) < 0.5 && isWalking)
            {
                isWalking = false;
                characterAnimator.ResetTrigger("Move");
                characterAnimator.SetTrigger("Idle");
                ps.Pause();
                targetIndicator.SetActive(false);
            }           

            if (isWalking)
            {
                RotateCharacter(targetPosition);
                RaycastHit hit;
                Vector3 origin = transform.position + Vector3.up * 2;
                if (Physics.Raycast(origin, transform.forward, out hit, 4))
                {
                    // Do something to prevent movement along the wall or mountain.
                    if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Checkpoint"))
                    {
                        
                        return;
                    }
                }

                isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

                /*if (isGrounded && velocity.y < 0)
                {
                    velocity.y = -2f;
                }*/

                Vector3 moveDirection = (targetPosition - transform.position).normalized;

                Vector3 move = moveDirection * speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, move.magnitude);

                //velocity.y += gravity * Time.deltaTime;
                
            }
        }        
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position + Vector3.up * 2, transform.position + transform.forward * 4 + Vector3.up * 2);
    }
    #endif
    public void Move(Vector3 point)
    {
        targetPosition = point;
        isWalking = true;
        characterAnimator.SetTrigger("Move");
        targetIndicator.SetActive(true);
        targetIndicator.transform.position = targetPosition;
        ps.Play();

    }

    public void DisableMove()
    {

        isWalking = false;
        targetIndicator.SetActive(false);
    }

    private void RotateCharacter(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0;
        if(direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
       

    }
}
