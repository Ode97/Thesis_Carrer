using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyBullet());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator DestroyBullet()
    {

        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
