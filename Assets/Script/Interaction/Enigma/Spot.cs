using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour
{
    public int objCode;
    private Vector3 initPosition;
    public Vector3 targetPosition;
    public float animSpeed = 1;
    public GameObject obstacle;
    private bool done = false;
    private bool end = false;
    // Start is called before the first frame update
    void Start()
    {
        initPosition = obstacle.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!end)
        {
            if (transform.childCount > 0)
            {
                var g = transform.GetChild(0).gameObject;

                if (g.GetComponent<EnigmaObj>()?.code == objCode)
                {

                    if (!done)
                    {
                        done = true;
                    }

                }
                else
                   g.transform.SetParent(null);

               
            }

            if (done)
            {
                if (targetPosition == transform.position)
                    end = true;

                Vector3 moveDirection = (targetPosition - transform.position).normalized;

                obstacle.transform.localPosition = Vector3.MoveTowards(obstacle.transform.localPosition, targetPosition, animSpeed * moveDirection.magnitude * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var g = other.gameObject;
        
        if (!done)
        {
            if(g.GetComponent<EnigmaObj>()?.code == objCode)
            {
                
                done = true;
            }
        }
    }    
}
