using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnigmaObj : MonoBehaviour
{
    
    public bool removeRb = false;
    private Enigma enigmaChecker;
    public Element element = Element.None;

    

    public int value = 0;
    

    private void Awake()
    {
        enigmaChecker = transform.parent.GetComponent<Enigma>();

        if (removeRb)
            GetComponent<Rigidbody>().isKinematic = true;
    }

    public void SetEnigmaChecker(Enigma enigma)
    {
        enigmaChecker = enigma;
    }

    public void Interaction(Element interactionElement)
    {

        if (interactionElement == element) {
            if (enigmaChecker.position)
                enigmaChecker.ElementPositionCheck(value);
            if (enigmaChecker.order)
                enigmaChecker.OrderCheck(value);
            if (enigmaChecker.activation)
            {
                enigmaChecker.ActiveAllCheck();
            }
        }
    }

}
