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
    public void WaterInteraction()
    {
        water.SetActive(true);
        animator.SetTrigger("rise");
    }

    override
    public void FireInteraction()
    {
        water.SetActive(false);
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
