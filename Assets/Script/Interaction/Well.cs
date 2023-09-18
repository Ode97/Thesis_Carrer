using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Well : MagicInteractable
{
    public GameObject water;

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
            water.SetActive(false);
        if (element.element == Element.Water)
            water.SetActive(true);
    }

}
