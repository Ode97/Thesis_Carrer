using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnigmaObj : MonoBehaviour
{
    public int code = 0;
    public bool removeRb = false;

    private void Start()
    {
        if (removeRb)
            GetComponent<Rigidbody>().isKinematic = true;
    }
}
