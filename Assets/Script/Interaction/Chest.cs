using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : InteractableObject
{
    public InventoryObject inventoryObject;
    private Animator animator;
    public GameObject pickupEffect;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    override
    public void Interaction()
    {
        animator.SetTrigger("open");
    }

    

    private void PickupEffect()
    {
        var effect = Instantiate(pickupEffect, transform);
        var pos = transform.position;
        effect.transform.position = new Vector3(pos.x, pos.y + 3f, pos.z);
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

    public override void EarthInteraction(GameObject obj, Vector3 pos)
    {
        
    }
}
