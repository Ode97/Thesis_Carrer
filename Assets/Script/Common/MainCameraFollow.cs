using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraFollow : MonoBehaviour
{
    public Transform targetRot;  // The character's transform to follow
    public Transform targetPos;
    public Vector3 offset_int;  // Offset between the camera and the character
    public Vector3 offset_view;
    private Vector3 offset;
    public float rotationSpeed = 5;
    public float positionSpeed = 5;

    private void Start()
    {
        offset = offset_int;
        ChangePos();

    }

    void Update()
    {
        ChangePos();
       
        
    }

    private void ChangePos()
    {
        Vector3 desiredPosition = targetPos.position - (targetRot.rotation * offset);
        //Vector3 desiredPosition = targetPos.position - offset;

        // Calcola la direzione corrente della telecamera
        Vector3 currentDirection = transform.forward;

        // Calcola la direzione desiderata della telecamera
        Vector3 desiredDirection = (desiredPosition - transform.position).normalized;

        // Calcola il coseno dell'angolo tra le due direzioni
        float angleCosine = Vector3.Dot(currentDirection, desiredDirection);

        // Interpolazione della posizione desiderata della telecamera
        transform.position = Vector3.Lerp(transform.position, desiredPosition, positionSpeed * Time.deltaTime);

        // Controlla se l'angolo di direzione è maggiore di un certo valore (ad esempio, 0.95 corrisponde a circa 18 gradi)
        if (angleCosine < 1.5f)
        {

            // Interpolazione della rotazione desiderata della telecamera
            Quaternion desiredRotation = Quaternion.LookRotation(targetPos.position - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
         }
    }

    public void SetMode(GameMode mode)
    {
        if (mode == GameMode.Movment || mode == GameMode.Interaction)
        {
            offset = offset_int;
            return;
        }

        offset = offset_view;
    }
}
