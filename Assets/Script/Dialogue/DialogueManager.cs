using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    public TextMeshProUGUI nameText;

    public TextMeshProUGUI dialogueText;

    public Animator animator;

    private Queue<string> sentences;

    private bool move = false;

    private GameObject[] fairies;
    private Vector3[] targets;

    private void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true);
        nameText.text = dialogue.name;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNext();
    }

    public void StartDialogue(Dialogue dialogue, GameObject[] fairies, Vector3[] target)
    {
        animator.SetBool("IsOpen", true);
        nameText.text = dialogue.name;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        
        this.fairies = fairies;
        this.targets = target;
        DisplayNext(fairies, target);
    }

    public void DisplayNext()
    {
        if (fairies.Length > 0)
        {
            DisplayNext(fairies, targets);
            return;
        }

        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeAnimation(sentence));
    }

    public void DisplayNext(GameObject[] fairies, Vector3[] target)
    {
        
        if (sentences.Count == 0)
        {
            
            EndDialogue(fairies, target);
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeAnimation(sentence));
    }

    IEnumerator TypeAnimation(string sentence)
    {
        dialogueText.text = "";
        foreach (char c in sentence.ToCharArray())
        {
            dialogueText.text += c;
            yield return null;
        }
    }

    private void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        
    }
    int i = 0;
    private void EndDialogue(GameObject[] fairies, Vector3[] target)
    {
        
        animator.SetBool("IsOpen", false);
       
            //go.transform.position = target[i];
        
            
        move = true;
        
        i = 0;
    }

    private void Update()
    {
        if (move)
        {
            foreach (GameObject go in fairies)
            {
                go.transform.localPosition = Vector3.MoveTowards(go.transform.localPosition, targets[i], 20 * Time.deltaTime);

                
                if(Vector3.Distance(go.transform.localPosition, targets[i]) < 1)
                {
                    move = false;
                    fairies.Initialize();
                    targets.Initialize();
                }
                i++;
            }
            i = 0;
            
        }
    }

}
