using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthPlant : InventoryObject
{
    [SerializeField]
    private float scale;
    [SerializeField]
    private int index;
    public override void Chosen()
    {
        EarthElement.earthObject = this.gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening("Reset", Reset);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetScale()
    {
        return new Vector3 (scale, scale, scale);
    }
    

    public int GetIndex()
    {
        return index;
    }

    private void Reset()
    {
        if (gameObject.layer == Constants.intObjLayer)
        {
            
            Destroy(gameObject);
        }
    }
}
