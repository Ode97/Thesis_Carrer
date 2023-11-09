using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;

public class WaterObj : InteractableObject
{
    private bool rise = false;
    private Vector3 initPosition;
    public Vector3 targetPosition;
    public float animSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem.MainModule main = outlineEffect.GetComponent<ParticleSystem>().main;
        main.startColor = UnityEngine.Color.blue;
        initPosition = transform.localPosition;
    }

    override
    // Update is called once per frame
    protected void Update()
    {
        base.Update();
        if (rise)
        {
            
            Vector3 moveDirection = (targetPosition - transform.localPosition).normalized;
            
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

        if (!rise)
        {
            
            rise = true;

            GetComponent<EnigmaObj>()?.Interaction(Element.Water);
            return true;
        }
        //else
            //rise = false;

        return true;
    }

    override
    public bool FireInteraction()
    {
        //water.SetActive(false);
        //rise = false;
        return false;
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
