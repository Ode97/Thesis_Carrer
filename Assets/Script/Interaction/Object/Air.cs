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
        initPos = transform.localPosition;
        initRot = transform.localRotation;
        element = Element.Air;

        if(movingPlatform && onlyUp)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ| RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            
        }
    }

    private bool remove = false;
    private GameObject toRemove;
    override
    protected void Update()
    {
        base.Update();
        if (onPlatform.Count > 0)
        {
            foreach (GameObject go in onPlatform)
            {
                if (Vector3.Distance(go.transform.position, transform.position) > 20)
                {
                    go.transform.SetParent(null);
                    remove = true;
                    toRemove = go;
                }
            }
            if (remove)
            {
                onPlatform.Remove(toRemove);
                remove = false;
            }
        }

        if (transform.position.y < initPos.y - 80)
        {
            transform.position = initPos;
            rb.velocity = Vector3.zero;
        } 
    }

    public bool onlyUp = false;
    override
    public bool AirInteraction()
    {   
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~GameManager.instance.ignoreLayer))
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
                    if(onlyUp)
                        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                    
                    //transform.rotation = new Quaternion(0, r.y, 0, r.w);
                    foreach (GameObject go in onPlatform)
                    {
                        
                        var rbo = go.GetComponent<Rigidbody>();
                        rbo.AddForce(direction, ForceMode.Force);
                        //go.transform.rotation = transform.rotation;
                    }
                }
            }           
        }
        return true;
    }

    public void StopMove()
    {
        if(movingPlatform && onlyUp)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }
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
        
        transform.localPosition = initPos;
        transform.localRotation = initRot;
    }

    public void SetInitPos(Vector3 p, Quaternion q)
    {
        initPos = p;
        initRot = q;
    }

    public Vector3 GetInitPos()
    {
        return initPos;
    }

    public Quaternion GetInitRot()
    {
        return initRot;
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


    private void OnCollisionExit(Collision collision)
    {
        
    }


}
