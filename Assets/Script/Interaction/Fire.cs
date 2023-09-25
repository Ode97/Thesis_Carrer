using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : InteractableObject
{
    public GameObject fireParticle;
    public bool distructible = false;
    // Start is called before the first frame update
    void Start()
    {
        fireParticle = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    override
    public void FireInteraction()
    {
        if (distructible)
            gameObject.SetActive(false);
        else
            fireParticle.SetActive(true);
    }

    override
    public void WaterInteraction()
    {
        fireParticle.SetActive(false);
    }

    public override void Interaction()
    {
        
    }

    public override void AirInteraction(Vector3 force)
    {
        
    }

    public override void EarthInteraction(GameObject obj, Vector3 pos)
    {
        
    }
}
