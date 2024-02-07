using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    public float waterForce = 1;
    private Rigidbody rb;
    [SerializeField]
    private GameObject water;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (water && transform.position.y - water.transform.position.y < 0.15)
        {
            
            rb.AddForce(water.transform.up * waterForce, ForceMode.Force);
        }
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<WaterObj>())
        {
            collision.collider.isTrigger = true;
            water = collision.gameObject;
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<WaterObj>())
        {
            other.isTrigger = true;
            water = other.gameObject;
        }
    }
}
