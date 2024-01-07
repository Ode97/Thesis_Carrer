using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EarthPlant : InventoryObject
{
    [SerializeField]
    private float scale;
    [SerializeField]
    private int index;
    public override void Chosen()
    {
        EarthElement.earthObject = gameObject;
        EventManager.TriggerEvent("Unselected");
        GetComponentInParent<Image>().color = Color.green;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening("Reset", Reset);
        EventManager.StartListening("Unselected", Unselected);
    }

    private void Unselected()
    {
        if(gameObject.layer == Constants.UILayer)
            GetComponentInParent<Image>().color = Color.white;
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
        if (gameObject.GetComponent<EnigmaObj>())
        {
            
            Destroy(gameObject);
        }
    }
}
