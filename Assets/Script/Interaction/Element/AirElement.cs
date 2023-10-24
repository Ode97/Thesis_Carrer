using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEditor;
using UnityEngine;

public class AirElement : MagicElement
{
    private bool clicked = false;
    private GameObject airEffect;
    private GameObject actualeffect;


    private void Start()
    {
        airEffect = Resources.Load<GameObject>("Magic circle 2 - air Variant");
    }
    override
    public void ApplyEffect()
    {

        GameManager.instance.stopLogic = true;
        StartCoroutine(Wait());
        actualeffect = Instantiate(airEffect, interactableObject.transform);
        actualeffect.transform.localPosition = new Vector3(0, -1, 0);
        actualeffect.transform.localScale = Vector3.one;

    }

    private void FixedUpdate()
    {
        if (clicked)
        {
            if (Input.GetMouseButtonDown(0))
            {
                
                clicked = false;
                StartCoroutine(Wait2());
                Destroy(actualeffect);
            }

            if (!interactableObject.AirInteraction())
            {
                clicked = false;
                GameManager.instance.stopLogic = false;
            }
            
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForEndOfFrame();
        clicked = true;
    }

    private IEnumerator Wait2()
    {
        yield return new WaitForEndOfFrame();
        GameManager.instance.stopLogic = false;
    }
}
