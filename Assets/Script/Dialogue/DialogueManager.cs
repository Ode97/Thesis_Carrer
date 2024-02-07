using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class DialogueManager : MonoBehaviour
{

    public TextMeshProUGUI nameText;

    public TextMeshProUGUI dialogueText;

    public Animator animator;

    private Queue<string> sentences;

    private bool end = false;

    private GameObject[] fairies = new GameObject[0];
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
        GameManager.instance.stopLogic = true;
        end = false;
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
        GameManager.instance.stopLogic = true;
        end = false;
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        
        this.fairies = fairies;
        i = 0;
        foreach (GameObject f in fairies)
        {

            f.GetComponent<Fairy>().SetTarget(target[i]);
            i++;
        }
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
        
        if (sentences.Count == 0)
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

    public void EndDialogue()
    {
        GameManager.instance.stopLogic = false;
        animator.SetBool("IsOpen", false);
        
    }
    int i = 0;
    private void EndDialogue(GameObject[] fa, Vector3[] target)
    {
        GameManager.instance.stopLogic = false;
        if (!end)
        {
            animator.SetBool("IsOpen", false);

            //go.transform.position = target[i];
            foreach (GameObject f in fa)
            {
                f.GetComponent<Fairy>().Move();
                i++;
            }
            i = 0;

            fairies = new GameObject[0];
            targets = new Vector3[0];
            end = true;
        }

    }

 

}
