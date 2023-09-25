using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enigma : MonoBehaviour
{
    public int objCode = 0;
    public GameObject obstacle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    private void OnTriggerEnter(Collider other)
    {
        var g = other.gameObject;
        Debug.Log("a " + other.gameObject.name);
        if (g.GetComponent<EnigmaObj>())
        {
            if (g.GetComponent<EnigmaObj>().code == objCode)
            {
                obstacle.SetActive(false);
            }
        }
    }
}
