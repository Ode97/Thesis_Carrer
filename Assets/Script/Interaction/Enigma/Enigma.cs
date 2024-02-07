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
    public bool diam = false;

    private int i = 0;
    private EnigmaObj[] objs;
    
    private void Start()
    {
        EventManager.StartListening("Reset", Reset);
        if (disableObstacle)
        {
            initPosition = obstacle.transform.localPosition;
        }
        else
            diam = objReward.activeSelf;

        var eo = GetComponentsInChildren<EnigmaObj>();

        objs = new EnigmaObj[eo.Length];

        objs = eo;
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
                objReward.GetComponent<MeshRenderer>().enabled = true;
                end = true;
                reward = false;
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
        var intObj = GetComponentsInChildren<InteractableObject>();
        
        foreach(InteractableObject o in intObj)
        {
            EventManager.TriggerEvent("WrongEnigma" + o.name);
        }
        
    }

    
  
    public void ElementPositionCheck(int code)
    {
        foreach (var s in spots)
        {
            Debug.Log(s.name);
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
       
        if (i == numbersOfElement)
        {
            
            complete = true;
        }
    }

    public void Deactivate()
    {
        i--;
        
    }

    public void Reset()
    {
        i = 0;
        /*var x = GetComponentsInChildren<InteractableObject>();
        foreach (InteractableObject io in x)
        {
            io.WaterInteraction();
            
        }*/
        foreach (EnigmaObj eo in objs)
            eo.SetActiveObj(true);

        complete = false;
        if (reward)
        {
            objReward.GetComponent<MeshRenderer>().enabled = diam;
        }
    }

    public bool[] IsComplete()
    {
        var x = new bool[2];
        if (reward)
        {
            
            x[1] = objReward.GetComponent<MeshRenderer>().enabled;
        }
        else
        {
            x[1] = false;
        }
        x[0] = complete;
        
        return x;
    }

    public void Complete(bool c, bool d)
    {
        //Debug.Log(gameObject.name + " completato: " + c + " statoReward: " + d);
        complete = c;
        if (reward)
        {                      
            if (c)
            {
                end = true;
                //objReward.SetActive(true);
                //Debug.Log(objReward.activeSelf);

                objReward.SetActive(d);
                objReward.GetComponent<MeshRenderer>().enabled = d;
                

            }

           
            
        }
    }
}
