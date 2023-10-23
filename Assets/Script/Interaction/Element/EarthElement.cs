using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EarthElement : MagicElement
{
    public GameObject initObj;
    public static GameObject earthObject;

    public void Start()
    {
        earthObject = initObj;
    }
    override
    public void ApplyEffect()
    {        
        interactableObject.EarthInteraction(earthObject, objectPosition);
    }

    
}
