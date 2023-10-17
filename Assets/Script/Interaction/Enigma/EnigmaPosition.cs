using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnigmaPosition : Enigma
{
    public GameObject obstacle;
    public int[] combination;
    private int i = 0;
    private Animator animator;

    void Start()
    {
       animator = obstacle.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    override
    public void WinnerCheck(int code)
    {
        if (i < combination.Length && code == combination[i])
        {
            obstacle.SetActive(false);
            
            i++;
        } else
            i = 0;
    }


    
}
