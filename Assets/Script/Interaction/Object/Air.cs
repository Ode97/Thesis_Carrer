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
    private Vector3 initPos;
    private Quaternion initRot;

    // Start is called before the first frame update
    void Start()
    {
        color = Color.grey;
        rb = GetComponent<Rigidbody>();
        initPos = transform.position;
        initRot = transform.rotation;
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

                direction = (moveDirection * 50);
                
                rb.AddForce(direction, ForceMode.Force);


                //var r = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(new Vector3(direction.x, transform.position.y, direction.z)), Time.deltaTime * 20);
                

                if (movingPlatform)
                {

                    //transform.rotation = new Quaternion(0, r.y, 0, r.w);
                    foreach (GameObject go in onPlatform)
                    {
                        Debug.Log(onPlatform.Count);
                        var rbo = go.GetComponent<Rigidbody>();
                        rbo.AddForce(direction, ForceMode.Force);
                        //go.transform.rotation = transform.rotation;
                    }
                }
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

    public override void Reset()
    {
        transform.position = initPos;
        transform.rotation = initRot;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var g = collision.gameObject;
        if (movingPlatform && !onPlatform.Contains(g) && !g.GetComponent<WaterObj>() && g.layer != Constants.terrainLayer && g.layer != 0)
        {            
            //collision.gameObject.transform.SetParent(transform);
            onPlatform.Add(collision.gameObject);
            if(g.layer == Constants.protagonistLayer)
                GameManager.instance.character.SetPlatform(gameObject);
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
        
    }


}
