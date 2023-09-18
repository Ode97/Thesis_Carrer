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
            }
        }
    }

    public void Move(Vector3 point)
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
