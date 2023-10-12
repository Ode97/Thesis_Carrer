using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Move : MonoBehaviour
{
    private float rotationSpeed = 5f;   
    private Animator characterAnimator; 

    private bool isWalking = false;   
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoTo(Vector3 point)
    {
        targetPosition = point;
        isWalking = true;
        characterAnimator.SetTrigger("Move");
        targetIndicator.SetActive(true);
        targetIndicator.transform.position = targetPosition;
        ps.Play();

    }

    private void RotateCharacter(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
}
