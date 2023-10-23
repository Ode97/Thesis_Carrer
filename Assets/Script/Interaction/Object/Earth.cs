using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earth : InteractableObject
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    override
    public bool EarthInteraction(GameObject obj, Vector3 pos)
    {
        GameObject t;
        if (gameObject.layer == LayerMask.NameToLayer("Terrain"))
            t = Instantiate(obj, transform);
        else
            t = Instantiate(obj);

        t.transform.position = pos;
        t.transform.localScale = new Vector3(15, 15, 15);
        t.transform.LookAt(GameManager.instance.character.transform);
        //GetComponent<EnigmaObj>()?.Interaction(Element.Earth);
        transform.GetComponent<Spot>()?.EnigmaSolve(obj);

        return true;
    }

    public override bool Interaction()
    {
        return false;
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
}
