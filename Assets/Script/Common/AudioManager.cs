using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    
    public AudioSource forest;
    public AudioSource lake;

    void Start()
    {
        forest.Play();
    }
}
