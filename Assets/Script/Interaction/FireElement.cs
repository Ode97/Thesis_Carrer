using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireElement : MagicElement
{
    public override void ApplyEffect()
    {
        interactableObject.FireInteraction();
    }
}
