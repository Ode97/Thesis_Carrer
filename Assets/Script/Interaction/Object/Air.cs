using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Air : InteractableObject
{
    private Vector3 destination;
    private Vector3 direction;
    private Rigidbody rb;
    public bool movingPlatform = false;
    private List<GameObject> onPlatform = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        color = Color.grey;
        rb = GetComponent<Rigidbody>();
    }

    private bool remove = false;
    private GameObject toRemove;
    override
    protected void Update()
    {
        base.Update();
        if(onPlatform.Count > 0)
        {
            foreach( GameObject go in onPlatform )
            {
                if (Vector3.Distance(go.transform.position, transform.position) > 20)
                {
                    go.transform.SetParent(null);
                    remove = true;
                    toRemove = go;
                }
            }
            if(remove)
            {
                onPlatform.Remove(toRemove);
                remove = false;
            }
        }
    }

    public bool onlyUp = false;
    override
    public bool AirInteraction()
    {   
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            destination = hit.point;
            if (hit.collider.gameObject == gameObject)
            {                
                return true;
            }
            if (hit.collider.gameObject.layer != Constants.protagonistLayer)
            {
                Vector3 moveDirection;
                if (onlyUp)
                    moveDirection = new Vector3(0, destination.y - transform.position.y, 0).normalized;
                else
                    moveDirection = (destination - transform.position).normalized;

                direction = moveDirection * 50;
                rb.AddForce(direction, ForceMode.Force);
                GameManager.instance.character.AddForce(direction);
            }           
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
        if (movingPlatform && collision.gameObject.layer != 12 && !collision.gameObject.GetComponent<WaterObj>() && !collision.gameObject.GetComponent<Terrain>())
        {
            Debug.Log(collision.gameObject.name);

            //collision.gameObject.transform.SetParent(transform);
            onPlatform.Add(collision.gameObject);
            GameManager.instance.character.SetCheckpoint(transform.position);
        }        
    }

    /*private void DisablePlatform()
    {
        if(movingPlatform)
        {
            GameManager.instance.character.transform.SetParent(transform);
        }else
            GameManager.instance.character.transform.SetParent(null);
    }*/

    private void OnCollisionExit(Collision collision)
    {
        //if (collision.gameObject.layer != LayerMask.NameToLayer("Terrain") && !collision.gameObject.GetComponent<WaterObj>())
        //    collision.gameObject.transform.SetParent(null);
    }
}
