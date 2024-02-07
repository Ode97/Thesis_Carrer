using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Book : InteractableObject
{
    public bool mapBook = false;
    [SerializeField]
    private Button map;
    private DialogueTrigger dialogueTrigger;
    private bool taken = false;
    int index = 0;
    public override bool AirInteraction()
    {
        //throw new System.NotImplementedException();
        return false;
    }

    public override bool EarthInteraction(GameObject obj, Vector3 pos)
    {
        return false;
    }

    public override bool FireInteraction()
    {
        return false;
    }

    public override bool Interaction()
    {
        if (mapBook)
        {
            map.interactable = true;
            map.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
            map.GetComponent<MenuButton>().enabled = true;
        }
        taken = true;
        gameObject.SetActive(false);
        dialogueTrigger.TriggerDialogue();
        return true;
        
    }

    public bool Taken()
    {
        if (mapBook)
        {
            map.interactable = true;
            map.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
            map.GetComponent<MenuButton>().enabled = true;
        }
        taken = true;
        gameObject.SetActive(false);
        return true;

    }

    public bool IsTaken()
    {
        return taken;
    }

    public override void Reset()
    {
        gameObject.SetActive(true);
        taken = false;
    }

    public override bool WaterInteraction()
    {
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        dialogueTrigger = GetComponent<DialogueTrigger>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (mapBook)
        {
            map.interactable = true;
            map.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
            map.GetComponent<MenuButton>().enabled = true;
        }
        taken = true;
        gameObject.SetActive(false);
        dialogueTrigger.TriggerDialogue();
    }
}
