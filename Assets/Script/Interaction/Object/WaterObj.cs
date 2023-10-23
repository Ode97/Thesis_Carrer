using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;

public class WaterObj : InteractableObject
{
    public GameObject water;
    private bool rise = false;
    private Vector3 initPosition;
    public Vector3 targetPosition;
    public float animSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        water = transform.GetChild(0).gameObject;
        
        initPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (rise)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;

            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, animSpeed * moveDirection.magnitude * Time.deltaTime);
        }
        else
        {
            Vector3 moveDirection = (initPosition - transform.position).normalized;

            transform.localPosition = Vector3.MoveTowards(transform.localPosition, initPosition, animSpeed * moveDirection.magnitude * Time.deltaTime);
        }
    }

    override
    public bool WaterInteraction()
    {
        //water.SetActive(true);
        //animator.SetTrigger("rise");
        rise = true;

        GetComponent<EnigmaObj>()?.Interaction(Element.Water);
        return true;
    }

    override
    public bool FireInteraction()
    {
        //water.SetActive(false);
        rise = false;
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
