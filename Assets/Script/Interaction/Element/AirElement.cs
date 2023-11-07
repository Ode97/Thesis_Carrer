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
        airEffect = Resources.Load<GameObject>("air_effect");
    }
    override
    public void ApplyEffect()
    {

        GameManager.instance.stopLogic = true;
        StartCoroutine(Wait());
        actualeffect = Instantiate(airEffect);
        actualeffect.transform.localScale = interactableObject.transform.localScale - Vector3.one;
        //actualeffect.transform.position = interactableObject.transform.position - new Vector3(0, -1, 0);
        //actualeffect.transform.localScale = Vector3.one;

    }

    private void FixedUpdate()
    {
        if (clicked)
        {
            Vector3 pos = interactableObject.transform.position;
            actualeffect.transform.position = new Vector3(pos.x, pos.y - 0.5f, pos.z);
            Debug.Log("start" + interactableObject.name);
            

            if (!interactableObject.AirInteraction())
            {
                Debug.Log("false " + interactableObject.name);
                clicked = false;
                GameManager.instance.stopLogic = false;
                Destroy(actualeffect);
                return;
            }

            character.DisableElement();
            if (Input.GetMouseButtonDown(0) || interactableObject.IsOnView() || !GameManager.instance.IsInteraction())// || interactableObject.GetComponent<EnigmaObj>().waterRespawn)
            {
                Debug.Log("stop" + interactableObject.name);
                clicked = false;
                StartCoroutine(Wait2());
                Destroy(actualeffect);
            }

            
            
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForFixedUpdate();
        clicked = true;
    }

    private IEnumerator Wait2()
    {
        yield return new WaitForFixedUpdate();
        GameManager.instance.stopLogic = false;
    }
}
