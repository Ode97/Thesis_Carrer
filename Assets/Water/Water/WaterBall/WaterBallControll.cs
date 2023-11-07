using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBallControll : MonoBehaviour
{
    [SerializeField] bool _update;
    [SerializeField] Transform _CreationPoint;
    [SerializeField] WaterBall WaterBallPrefab;
    WaterBall waterBall = null;

    private void Start()
    {
        _CreationPoint = GameManager.instance.character.transform;
    }
    private void Update()
    {
        if (!_update)
        {
            return;
        }




        if (!busy) 
        { 

            StartCoroutine(Throw());
        }
        
    }


    private bool busy = false;
    private IEnumerator Throw()
    {
        CreateWaterBall();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        busy = true;
        yield return new WaitForSeconds(0.5f);
        busy = false;
        if (Physics.Raycast(ray, out hit))
        {
            if (waterBall != null)
            {
                ThrowWaterBall(hit.point);
            }
        }
    }
    /*public bool WaterBallCreated()
    {
        
        return waterBall != null;
    }*/
    public void CreateWaterBall()
    {
        waterBall = Instantiate(WaterBallPrefab, _CreationPoint.position + Vector3.up * 10, Quaternion.identity);
        waterBall.WaterControl(this);
    }

    public void ThrowWaterBall(Vector3 pos)
    {
        waterBall.Throw(pos);
    }
}
