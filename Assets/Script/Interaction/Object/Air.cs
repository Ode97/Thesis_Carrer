using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Air : InteractableObject
{

    private Vector3 destination;
    private Vector3 direction;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        


    }

    override
    public bool AirInteraction()
    {   

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            destination = hit.point;

            Vector3 moveDirection = (destination - transform.position).normalized;

            direction = moveDirection * rb.mass * 10;
            rb.AddForce(direction, ForceMode.Force);
            //transform.position = Vector3.MoveTowards(transform.position, destination, direction.magnitude);

            //GameManager.instance.MoveCharacter(destination, direction, gameObject);

        }

        return true;
    }

    public override bool Interaction()
    {
        return false;
    }

    public override bool WaterInteraction()
    {
        return false;
    }

    public override bool FireInteraction()
    {
        return false;
    }

    public override bool EarthInteraction(GameObject obj, Vector3 pos)
    {
        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer != LayerMask.NameToLayer("Terrain") && collision.gameObject.layer != LayerMask.NameToLayer("Default")  && !collision.gameObject.GetComponent<WaterObj>())
            collision.gameObject.transform.SetParent(transform);
    }

    private void OnCollisionExit(Collision collision)
    {
        //if (collision.gameObject.layer != LayerMask.NameToLayer("Terrain") && !collision.gameObject.GetComponent<WaterObj>())
        //    collision.gameObject.transform.SetParent(null);
    }
}
