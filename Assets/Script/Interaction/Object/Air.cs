using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Air : InteractableObject
{

    private Vector3 destination;
    public Vector3 direction;


    // Start is called before the first frame update
    void Start()
    {
        
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


            direction = moveDirection * 5 * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, destination, direction.magnitude);
            GameManager.instance.MoveCharacter(destination, direction);
            //Debug.Log("moving " + name);
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
}
