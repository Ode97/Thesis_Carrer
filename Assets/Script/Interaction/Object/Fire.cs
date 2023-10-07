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
        EventManager.StartListening("WrongFireEnigma", CloseFire);
        fireParticle = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    override
    public bool FireInteraction()
    {
        if (distructible)
            gameObject.SetActive(false);
        else
        {
            fireParticle.SetActive(true);
        }

        GetComponent<EnigmaObj>()?.Interaction(Element.Fire);
        return true;
    }

    private void CloseFire()
    {
        WaterInteraction();
    }

    override
    public bool WaterInteraction()
    {
        fireParticle.SetActive(false);
        return true;
        
    }

    public override bool Interaction()
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
