using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    private DialogueManager manager;
    private void Start()
    {
        manager = FindObjectOfType<DialogueManager>();
    }

    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        manager.StartDialogue(dialogue);
    }
}
