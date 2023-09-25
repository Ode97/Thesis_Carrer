using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthElement : MagicElement
{
    public GameObject tree;

    override
    public void ApplyEffect()
    {
        interactableObject.EarthInteraction(tree, pos);
    }
}
