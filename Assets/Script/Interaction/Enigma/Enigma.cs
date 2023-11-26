using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public  class Enigma : MonoBehaviour
{
    [SerializeField]
    private bool disableObstacle = false;
    [SerializeField]
    private bool reward = false;
    [SerializeField]
    private GameObject objReward;
    [SerializeField]
    private GameObject obstacle;
    [SerializeField]
    private GameObject water;

    [SerializeField]
    public bool order = false;
    [SerializeField]
    public bool position = false;
    [SerializeField]
    public bool activation = false;

    private bool complete = false;
    protected Vector3 initPosition;
    public Vector3 targetPosition;
    public int animSpeed = 5;

    [SerializeField]
    private List<Spot> spots;
    public int elementsToOrder;
    public int numbersOfElement;
    //public int signalToResolve = 0;

    private int i = 0;
    


    private void Start()
    {
        if(disableObstacle)
            initPosition = obstacle.transform.localPosition;
    }

    private bool end = false;
    private void Update()
    {
        if (!end)
        {
            if (complete && disableObstacle)
            {
                
                //Vector3 moveDirection = (targetPosition - obstacle.transform.localPosition).normalized;

                obstacle.transform.localPosition = Vector3.MoveTowards(obstacle.transform.localPosition, targetPosition, animSpeed * 20 * Time.deltaTime);
            }
            else if (!complete && disableObstacle)
            {
                //Vector3 moveDirection = (initPosition - obstacle.transform.localPosition).normalized;
                obstacle.transform.localPosition = Vector3.MoveTowards(obstacle.transform.localPosition, initPosition, animSpeed * 20 * Time.deltaTime);
            }

            if (reward && complete)
            {
                objReward.SetActive(true);
                end = true;
            }
        }
    }

    public void OrderCheck(int value)
    {
        if (i == value)
        {
            if (i == elementsToOrder - 1)
            {
                complete = true;
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
        EventManager.TriggerEvent("WrongFire" + gameObject.name);
    }

    
  
    public void ElementPositionCheck(int code)
    {

        /*if (i < elements.Length && code == elements[i])
        {


            i++;
            if (i == elements.Length)
                complete = true;
        }*/

        
        foreach (var s in spots)
        {
            if (s.isCorrect())
            {
                i++;
                if(i == spots.Count)
                    complete = true;
            }
        }

        i = 0;

    }

    public void ActiveAllCheck()
    {
        i++;
        Debug.Log(i + " " + numbersOfElement);
        if (i == numbersOfElement)
        {
            complete = true;
        }
    }

}
