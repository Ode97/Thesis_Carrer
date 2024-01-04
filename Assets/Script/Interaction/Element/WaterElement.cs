using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterElement : MagicElement
{

    public override void ApplyEffect()
    {
        interactableObject.WaterInteraction();
    }
}
