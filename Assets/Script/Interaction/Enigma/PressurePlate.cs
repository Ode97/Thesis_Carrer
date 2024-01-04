using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public GameObject obstacle;
    private bool open = false;
    private Vector3 initPosition;
    private Quaternion initRotation;
    public Vector3 targetPosition;
    public float animSpeed = 1;
    public bool enigma = false;
    private int i = 0;
    EnigmaObj enim;
    private Rigidbody rb;
    public bool returnBack = false;
    public bool physics = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!physics)
        {
            initPosition = obstacle.transform.localPosition;
            initRotation = obstacle.transform.localRotation;
        }
        else
        {
            initPosition = obstacle.transform.position;
            initRotation = obstacle.transform.rotation;
        }
        initRotation = obstacle.transform.rotation;
        enim = GetComponent<EnigmaObj>();
        if(physics)
            rb = obstacle.GetComponent<Rigidbody>();
    }

    private bool back = false;
    public bool stopWhenComplete = false;
    // Update is called once per frame
    void Update()
    {
        if (open)
        {
            if (!physics)
            {
                Vector3 moveDirection = (targetPosition - obstacle.transform.localPosition).normalized;

                obstacle.transform.localPosition = Vector3.MoveTowards(obstacle.transform.localPosition, targetPosition, animSpeed * moveDirection.magnitude * Time.deltaTime);
            }
            back = true;
        }
        else if(returnBack)
        {

            ComeBack();
            back = false;
        }
    }

    private void ComeBack()
    {
        if (!physics)
        {
            Vector3 moveDirection = (initPosition - obstacle.transform.localPosition).normalized;
            obstacle.transform.localPosition = Vector3.MoveTowards(obstacle.transform.localPosition, initPosition, animSpeed * moveDirection.magnitude * Time.deltaTime);


            if (back)
            {

                enim.GetComponent<InteractableObject>()?.Reset();
                enim.enigmaChecker.Reset();
                //enim.SetActiveObj(true);
                Debug.Log(enim.enigmaChecker.name);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        
        if (!physics)
        {
            var g = other.gameObject;
            Debug.Log("bbbbbbb");
            if (g.GetComponent<Rigidbody>() && g.GetComponent<Rigidbody>().mass > 0.5f && !stop)
            {
                Debug.Log("cccccc");
                i++;
                if (enigma)
                {
                    Debug.Log("dddddd");
                    enim.Interaction(Element.None);
                    stop = true;

                }
                else
                {
                    
                    open = true;
                }

            }
        }
    }

    //Trigger used for enable also navmash agent activation
    private void OnTriggerEnter(Collider other)
    {

        if (!physics)
        {
            var g = other.gameObject;
            if (g.GetComponent<Rigidbody>() && g.GetComponent<Rigidbody>().mass > 0.5f && !stop)
            {
                i++;
                if (enigma)
                {
                    Debug.Log(other.gameObject.name + " " + transform.name);
                    enim.Interaction(Element.None);
                    stop = true;

                }
                else
                {

                    open = true;
                }

            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        var g = collision.gameObject;
        if (g.GetComponent<Rigidbody>() && g.GetComponent<Rigidbody>().mass > 0.5f && !stop)
        {
            i--;

            if (i == 0)
            {
                open = false;
            }

        }
    }

    private bool stop = false;
    private void OnCollisionStay(Collision other)
    {
        if (physics)
        {
            var g = other.gameObject;
            
            if (g.GetComponent<Rigidbody>() && g.GetComponent<Rigidbody>().mass > 0.5f && !stop)
            {

                i++;
                if (enigma)
                {

                    enim.Interaction(Element.None);
                    stop = true;

                }
                else
                {
                    if (physics)
                    {
                        if (!stopClone)
                            StartCoroutine(Clone(obstacle));

                        Vector3 moveDirection = (targetPosition - obstacle.transform.localPosition).normalized;

                        var v = new Vector3(0, 0, moveDirection.x);

                        rb.velocity = v * animSpeed;
                    }
                    open = true;
                }

            }
        }
    }

    private bool stopClone = false;
    private IEnumerator Clone(GameObject g)
    {
        stopClone = true;
        yield return new WaitForSeconds(0.5f);
        stopClone = false;
        obstacle = Instantiate(g, initPosition, initRotation, g.transform.parent);
        
        rb = obstacle.GetComponent<Rigidbody>();
        rb.isKinematic = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        var g = collision.gameObject;
        if (g.GetComponent<Rigidbody>() && g.GetComponent<Rigidbody>().mass > 0.5f && !stop)
        {
            i--;
            
            if (i == 0)
            {                
                open = false;
            }
            
        }
    }

    /*private void OnTriggerExit(Collider other)
    {
        Debug.Log("fuori " + other.gameObject.name);
        enigmaChecker.NotOnSpot();
    }*/
}

