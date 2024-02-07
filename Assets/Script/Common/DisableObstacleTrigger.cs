using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisableObstacleTrigger : MonoBehaviour
{
    [SerializeField] 
    private GameObject obstacle;
    [SerializeField]
    private Vector3 target;
    private bool move = false;

    private void Update()
    {
        if (move)
        {
            obstacle.transform.localPosition = Vector3.MoveTowards(obstacle.transform.localPosition, target, 20 * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == Constants.bossTriggerLayer)
        {

            move = true;
        }
    }
}
