using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttack : MonoBehaviour
{
    public Element element;
    private Vector3 hitPoint;
    private bool destroy = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(element == Element.Air)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Enemy")))
            {
                hitPoint = hit.collider.transform.position;
            }
            
            transform.position = Vector3.MoveTowards(transform.position, hitPoint, Time.deltaTime * 10);
            if (!destroy)
            {
                StartCoroutine(DestroyBullet());
                destroy = true;
            }
        }
    }

    private IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
