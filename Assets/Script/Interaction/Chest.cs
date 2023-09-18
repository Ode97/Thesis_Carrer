using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : PushObject
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
    public void Interaction(Vector3 point)
    {
        base.Interaction(point);
        animator.SetTrigger("open");
    }

    

    private void PickupEffect()
    {
        var effect = Instantiate(pickupEffect, transform);
        var pos = transform.position;
        effect.transform.position = new Vector3(pos.x, pos.y + 3f, pos.z);
    }
}
