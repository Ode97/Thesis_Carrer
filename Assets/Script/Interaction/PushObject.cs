using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObject : MagicInteractable
{

    private Vector3 direction;
    private bool pushing = false;
    private bool stop = false;

  
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (pushing)
        {
            transform.Translate(direction * Constants.airPower * Time.deltaTime);
            if(!stop)
                StartCoroutine(Stop());
        }
    }

    private IEnumerator Stop()
    {
        stop = true;
        yield return new WaitForSeconds(Constants.airTime);
        pushing = false;
        stop = false;
    }

    override
    public void MagicInteraction(MagicElement element)
    {
        var character = GameManager.instance.character;

        if (element.element == Element.Air)
        {
            direction = (transform.position - character.transform.position).normalized;
            pushing = true;
        }
    }
}
