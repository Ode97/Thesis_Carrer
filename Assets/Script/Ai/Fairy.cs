using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fairy : MonoBehaviour
{
    private bool move = false;
    private Vector3 target = new Vector3();
    private Vector3 initPos = new Vector3();
    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.position;
        target = transform.position;
        EventManager.StartListening("Reset", Reset);
    }

    // Update is called once per frame
    void Update()
    {
        if (move)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, 20 * Time.deltaTime);
            Vector3 direction = target - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime);

            if (Vector3.Distance(transform.localPosition, target) < 1)
            {
                move = false;
                
            }
        }
        transform.LookAt(GameManager.instance.character.transform.position);
    }

    public void Move()
    {
        move = true;
        
    }

    public void SetTarget(Vector3 t) { target = t; }
    public Vector3 GetTarget() { return target; }

    public void Reset()
    {
        transform.position = initPos;
    }
}
