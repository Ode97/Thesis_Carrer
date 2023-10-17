using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MainCameraFollow : MonoBehaviour
{
    public Transform target;  // The character's transform to follow
   // public Transform targetPos;
    public Vector3 offset_move;  // Offset between the camera and the character
    public Vector3 offset_view;
    private Vector3 offset;
    public float rotationSpeed = 5;
    public float positionSpeed = 5;

    public float rotationSpeedView = 5.0f;
    private CameraMode mode;
    private Quaternion lastRotation;

    private void Awake()
    {
        offset = offset_move;
        mode = CameraMode.Movment;
        ChangePos();

    }

    void Update()
    {
        if (mode == CameraMode.View)
            View();
        else
            ChangePos();
    }

    private void ChangePos()
    {

        Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);

        //transform.LookAt(target);

        SetPosition();

    }

    private void View() 
    {

        SetPosition();
            
        // Ottieni la posizione del mouse
        Vector3 mousePosition = Input.mousePosition;
        // Ottieni le dimensioni dello schermo
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Calcola la percentuale di spostamento del mouse rispetto al bordo dello schermo
        float mouseXPercentage = mousePosition.x / screenWidth;
        float mouseYPercentage = mousePosition.y / screenHeight;

        // Imposta le soglie per attivare lo spostamento della camera quando il mouse si avvicina ai bordi
        float edgeThreshold = 0.1f; // Valore da regolare in base alla sensibilità desiderata

        // Calcola la differenza tra la posizione del mouse e la posizione centrale dello schermo
        float mouseXDelta = mouseXPercentage - 0.5f;
        float mouseYDelta = -mouseYPercentage + 0.5f;

        // Imposta l'angolo di rotazione intorno agli assi X e Z rispetto alla posizione corrente della camera
        float pitch = 90.0f * mouseYDelta;
        float yaw = 90.0f * mouseXDelta;
        
        // Calcola l'angolo di rotazione solo se il mouse è vicino ai bordi
        if (mouseYPercentage < edgeThreshold || mouseYPercentage > 1 - edgeThreshold)
        {
            if (transform.rotation.eulerAngles.x > 300 || transform.rotation.eulerAngles.x < 60)
                transform.RotateAround(target.transform.position, transform.right, pitch * rotationSpeedView * Time.deltaTime);
            else
            {
                transform.rotation = lastRotation;
                Debug.Log(lastRotation.eulerAngles);
            }
        }
        else if(mouseXPercentage < edgeThreshold || mouseXPercentage > 1 - edgeThreshold)
        {
            
            target.transform.RotateAround(target.transform.position, Vector3.up, yaw * rotationSpeedView * Time.deltaTime);
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, target.transform.eulerAngles.y, transform.eulerAngles.z);
        }

        if (transform.rotation.eulerAngles.x > 300 || transform.rotation.eulerAngles.x < 60)
            lastRotation = transform.rotation;
    }



    

    private void SetPosition()
    {
        Vector3 desiredPosition;
        if (mode == CameraMode.View)
        {
            desiredPosition = target.position + (target.rotation * offset);
        }else
        //Vector3 desiredPosition = targetP + (target.rotation * offset);
            desiredPosition = target.position + offset;

        // Interpolazione della posizione desiderata della telecamera
        transform.position = Vector3.Lerp(transform.position, desiredPosition, positionSpeed * Time.deltaTime);
    }

    public void SetMode(CameraMode newMode)
    {
        mode = newMode;
        if (mode == CameraMode.Movment)
        {
            offset = offset_move;
            return;
        }
        
        offset = offset_view;
        transform.rotation = Quaternion.LookRotation(target.forward);
        transform.Rotate(target.position, Quaternion.Angle(transform.rotation, target.rotation));
    }
}
