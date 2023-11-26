using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public GameObject obstacle;
    private bool open = false;
    private Vector3 initPosition;
    public Vector3 targetPosition;
    public float animSpeed = 1;
    public bool enigma = false;
    private int i = 0;
    EnigmaObj enim;

    // Start is called before the first frame update
    void Start()
    {
        //enigmaChecker = transform.parent.GetComponent<Enigma>();
        //animator = obstacle.GetComponent<Animator>();
        initPosition = obstacle.transform.localPosition;
        enim = GetComponent<EnigmaObj>();
    }

    // Update is called once per frame
    void Update()
    {
        if (open)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            
            obstacle.transform.localPosition = Vector3.MoveTowards(obstacle.transform.localPosition, targetPosition, animSpeed * moveDirection.magnitude * Time.deltaTime);
        }
        else
        {
            Vector3 moveDirection = (initPosition - transform.position).normalized;
            
            obstacle.transform.localPosition = Vector3.MoveTowards(obstacle.transform.localPosition, initPosition, animSpeed * moveDirection.magnitude * Time.deltaTime);
        }
    }

    private bool stop = false;
    private void OnCollisionEnter(Collision other)
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
                open = true;
            }
            
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        var g = collision.gameObject;
        if (g.GetComponent<Rigidbody>() && g.GetComponent<Rigidbody>().mass > 0.5f && !stop)
        {
            i--;
            if(i == 0)
                open = false;
            
        }
    }

    /*private void OnTriggerExit(Collider other)
    {
        Debug.Log("fuori " + other.gameObject.name);
        enigmaChecker.NotOnSpot();
    }*/
}

