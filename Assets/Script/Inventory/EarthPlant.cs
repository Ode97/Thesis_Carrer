using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthPlant : InventoryObject
{
    [SerializeField]
    private float scale;
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

    public Vector3 GetScale()
    {
        return new Vector3 (scale, scale, scale);
    }
}
