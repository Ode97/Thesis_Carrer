using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnigmaObj : MonoBehaviour
{
    public int code = 0;
    public bool removeRb = false;
    private Enigma enigmaChecker;
    public Element element = Element.None;

    private void Start()
    {
        enigmaChecker = transform.parent.GetComponent<Enigma>();

        if (removeRb)
            GetComponent<Rigidbody>().isKinematic = true;
    }

    public void Interaction(Element interactionElement)
    {
        
        if(interactionElement == element)
            enigmaChecker.WinnerCheck(code);

    }

}
