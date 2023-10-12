using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EnigmaOrder : Enigma
{
    //[SerializeField] private int[] trial;
    [SerializeField] private int length;
    private int i = 0;

    void Start(){
        //levelManager.enigmaChecker = this;
    }

    override
    public void WinnerCheck(int code)
    {
        //trial.SetValue(code, i);
        if (i == code)
        {
            if (i == length-1)
            {
                Debug.Log("vinto");
                return;
            }
            i++;
        }
        else
        {
            i = 0;
            StartCoroutine(WaitError());
        }
    }

    private IEnumerator WaitError()
    {
        yield return new WaitForSeconds(0.5f);
        EventManager.TriggerEvent("WrongFireEnigma");
    }

    public override void NotOnSpot()
    {
        throw new System.NotImplementedException();
    }
}
