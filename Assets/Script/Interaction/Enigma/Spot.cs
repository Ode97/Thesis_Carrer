using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour
{
    private Enigma enigmaChecker;
    // Start is called before the first frame update
    void Start()
    {
        enigmaChecker = transform.parent.GetComponent<Enigma>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        var g = other.gameObject;
        if (g.GetComponent<EnigmaObj>())
        {
            enigmaChecker.WinnerCheck(g.GetComponent<EnigmaObj>().code);
        }
    }
}
