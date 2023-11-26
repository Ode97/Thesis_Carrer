using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour
{
    public Enigma enigmaChecker;
    
    private bool ok = false;
    public int code;

    private void Awake()
    {
        //enigmaChecker = transform.parent.GetComponent<Enigma>();

    }

    private void Update()
    {       
        if (disappear)
        {
            if (actual.transform.localScale.magnitude > 0.01f)
            {
                actual.gameObject.transform.localScale = Vector3.Lerp(actual.gameObject.transform.localScale, Vector3.zero, Time.deltaTime * 5);            
            }
            else
            {
                Destroy(actual);
                disappear = false;               
            }
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
        disappear = true;
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

    private bool disappear = false;
    private GameObject actual;
    public void EnigmaSolve(GameObject g)
    {
        actual = g;
        var eo = actual.GetComponent<EnigmaObj>();
        if (eo.value == code)
        {
            ok = true;
            
            eo.SetEnigmaChecker(enigmaChecker);
            eo.Interaction(eo.element);
        }
        else
        {

            StartCoroutine(Wait());
        }           
    }
}
