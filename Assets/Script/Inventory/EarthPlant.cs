using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthPlant : InventoryObject
{
    public override void Chosen()
    {
        EarthElement.earthObject = this.gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
