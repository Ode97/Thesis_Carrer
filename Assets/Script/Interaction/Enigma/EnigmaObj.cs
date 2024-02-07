using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnigmaObj : MonoBehaviour
{
    
    public Enigma enigmaChecker;
    public Element element = Element.None;
    private Vector3 initPos;
    public int value = 0;
    

    private void Awake()
    {
        
        //enigmaChecker = transform.parent?.GetComponent<Enigma>();
        initPos = transform.position;
        
    }

    private void Update()
    {
        if (water)
        {
            WaterRespawn();
        }
    }

    private GameObject water;
    public bool waterRespawn = false;
    private void WaterRespawn()
    {

        if (Vector3.Distance(water.transform.position, transform.position) > 80)
        {
            Debug.Log(Vector3.Distance(water.transform.position, transform.position));
            water = null;
            return;
        }

        if (transform.position.y < water.transform.position.y - 5)
        {
            StartCoroutine(Respawn());
        }
        else if (transform.position.y > water.transform.position.y + 4)
        {
            water = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<WaterObj>())
        {
            water = other.gameObject;
        }
    }

    private IEnumerator Respawn()
    {
        waterRespawn = true;
        yield return new WaitForSeconds(2);
        waterRespawn = false;
        transform.rotation = Quaternion.identity;
        transform.position = initPos;
        

    }

    public void SetEnigmaChecker(Enigma enigma)
    {
        enigmaChecker = enigma;
    }

    public void SetActiveObj(bool c)
    {
        active = c;
    }

    private bool active = true;
    public void Interaction(Element interactionElement)
    {
       
        if (interactionElement == element && active) {
            
            if (enigmaChecker.position)
                enigmaChecker.ElementPositionCheck(value);
            if (enigmaChecker.order)
                enigmaChecker.OrderCheck(value);
            if (enigmaChecker.activation)
            {
                
                active = false;
                enigmaChecker.ActiveAllCheck();
            }
        }
    }

    public void ExitTriggerPlate()
    {
          
        if (enigmaChecker.activation)
        {

            active = true;
            enigmaChecker.Deactivate();
        }        
    }

}
