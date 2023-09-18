using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MagicInteractable
{
    public GameObject fireParticle;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    override
    public void MagicInteraction(MagicElement element)
    {
        if (element.element == Element.Fire)
            fireParticle.SetActive(true);
        if (element.element == Element.Water)
            fireParticle.SetActive(false);
    }

    
}
