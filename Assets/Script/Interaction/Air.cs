using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Air : InteractableObject
{

    private Vector3 direction;
    private Rigidbody rb;

  
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        


    }

    override
    public void AirInteraction(Vector3 force)
    {
        rb.AddForce(force, ForceMode.Impulse);
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

    public override void EarthInteraction(GameObject obj, Vector3 pos)
    {

    }
}
