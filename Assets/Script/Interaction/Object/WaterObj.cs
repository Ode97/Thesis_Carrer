using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterObj : InteractableObject
{
    public GameObject water;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        water = transform.GetChild(0).gameObject;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    override
    public bool WaterInteraction()
    {
        water.SetActive(true);
        animator.SetTrigger("rise");

        GetComponent<EnigmaObj>()?.Interaction(Element.Water);
        return true;
    }

    override
    public bool FireInteraction()
    {
        water.SetActive(false);
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
