using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour
{
    private Enigma enigmaChecker;
    
    private bool ok = false;
    public int code;

    private void Awake()
    {
        enigmaChecker = transform.parent.GetComponent<Enigma>();

    }

    private void OnTriggerEnter(Collider other)
    {
        var g = other.gameObject;
        
        g.GetComponent<EnigmaObj>()?.Interaction(g.GetComponent<EnigmaObj>().element);
        
    }    

    public bool isCorrect()
    {
        return ok;
    }

    public void EnigmaSolve(GameObject g)
    {
        if (g.GetComponent<EnigmaObj>().value == code)
        {
            ok = true;
            g.GetComponent<EnigmaObj>().SetEnigmaChecker(enigmaChecker);
            g.GetComponent<EnigmaObj>()?.Interaction(g.GetComponent<EnigmaObj>().element);
        }
    }
}
