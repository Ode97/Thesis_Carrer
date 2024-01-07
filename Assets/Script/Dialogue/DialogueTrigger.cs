using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    private DialogueManager manager;
    [SerializeField]
    private GameObject[] fairies;
    [SerializeField]
    private Vector3[] target;
    [SerializeField]
    private DialogueTrigger nextDialogue;
    private void Start()
    {
        manager = FindObjectOfType<DialogueManager>();
    }

    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        if (fairies.Length > 0)
        {
            manager.StartDialogue(dialogue, fairies, target);
        }
        else
            manager.StartDialogue(dialogue);
    }

    
}
