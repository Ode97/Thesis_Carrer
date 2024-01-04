using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttack : MonoBehaviour
{
    public Element element;
    private Vector3 hitPoint;
    private bool destroy = false;
    public AudioSource castAudio;
    public AudioSource impactAudio;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AudioCast()
    {
        castAudio.Play();
    }

    public void AudioImpact()
    {
        impactAudio.Play();
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
            
            transform.position = Vector3.MoveTowards(transform.position, hitPoint, Time.deltaTime * 50);
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
