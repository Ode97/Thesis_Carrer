using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<InventoryObject> inventory;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddObject(InventoryObject inventoryObject)
    {

    }

    public InventoryObject GetObject(int index)
    {
        
        return inventory[index];
    }
}
