using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class AirElement : MagicElement
{
    private bool clicked = false;

    override
    public void ApplyEffect()
    {

        GameManager.instance.stopLogic = true;
        StartCoroutine(Wait());
       

    }

    private void FixedUpdate()
    {
        if (clicked)
        {
            if (Input.GetMouseButtonDown(0))
            {
                
                clicked = false;
                StartCoroutine(Wait2());
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
