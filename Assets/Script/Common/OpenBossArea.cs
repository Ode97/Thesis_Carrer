using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class OpenBossArea : MonoBehaviour
{
    [SerializeField]
    private int requiredDiamonds = 0;
    [SerializeField]
    private GameObject obstacle;
    [SerializeField]
    private Vector3 targetPos;

    private bool move = false;
    private void Update()
    {
        if (move)
        {
            Vector3 moveDirection = (targetPos - obstacle.transform.localPosition).normalized;
            obstacle.transform.localPosition = Vector3.MoveTowards(obstacle.transform.localPosition, targetPos, 10 * moveDirection.magnitude * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.layer == Constants.protagonistLayer)
        {
            if (other.gameObject.GetComponent<Character>().GetDiamonds() >= requiredDiamonds)
            {
                move = true;
            }
        }
    }
}
