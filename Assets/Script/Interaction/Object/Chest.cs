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

    override
    public bool Interaction()
    {
        animator.SetTrigger("open");
        return true;
    }

    

    private void PickupEffect()
    {
        var effect = Instantiate(pickupEffect, transform);
        var pos = transform.position;
        effect.transform.position = new Vector3(pos.x, pos.y + 8f, pos.z);
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

    public override bool EarthInteraction(GameObject obj, Vector3 pos)
    {
        return false;
    }

}
