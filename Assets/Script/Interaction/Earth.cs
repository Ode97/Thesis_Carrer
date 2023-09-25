using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earth : InteractableObject
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    override
    public void EarthInteraction(GameObject obj, Vector3 pos)
    {
        var t = Instantiate(obj);
        t.transform.position = pos;
        t.transform.localScale = new Vector3(11, 11, 11);
    }

    public override void Interaction()
    {
        
    }

    public override void WaterInteraction()
    {
        
    }

    public override void FireInteraction()
    {
        
    }

    public override void AirInteraction(Vector3 force)
    {
        
    }
}
