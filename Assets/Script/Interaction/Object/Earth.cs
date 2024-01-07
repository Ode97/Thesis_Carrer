using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Earth : InteractableObject
{
    private Vector3 scaleTarget;
    private GameObject t;

    void Start()
    {        
        color = Color.green;
        

    }

    override
    protected void Update()
    {
        base.Update();
        
        if (scaleTrigger)
        {
            if (t.transform.localScale.magnitude < scaleTarget.magnitude - 0.01f)
            {
                t.transform.localScale = Vector3.Lerp(t.transform.localScale, scaleTarget, Time.deltaTime * 5);
            }
            else
            {
                
                scaleTrigger = false;
            }
        }
    }

    private bool scaleTrigger = false;
    override
    public bool EarthInteraction(GameObject obj, Vector3 pos)
    {

        /*if (gameObject.layer == Constants.terrainLayer)
            t = Instantiate(obj, transform);
        else*/
        
        t = Instantiate(obj);
        scaleTarget = t.GetComponent<EarthPlant>().GetScale();

        t.transform.position = pos;
        
        t.transform.localScale = new Vector3(0, 0, 0);
        var c = GameManager.instance.character.transform.position;
        t.transform.LookAt(new Vector3(c.x, t.transform.position.y, c.z));
        
        t.layer = Constants.plantLayer;
        
        transform.GetComponent<Spot>()?.EnigmaSolve(t);

        scaleTrigger = true;

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

    public override bool AirInteraction()
    {
        return false;
    }

    public override void Reset()
    {
        
    }
}
