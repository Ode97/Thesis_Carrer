using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MagicInteractable : InteractableObject
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    override
    public void Interaction(Vector3 point)
    {
        var character = GameManager.instance.character;
        
        if (!character.isActiveElement())
        {
            return;
        }

        character.SetMagicPoint(point);
        MagicElement e = character.getActualElement();
        
        MagicInteraction(e);
    }


    public abstract void MagicInteraction(MagicElement element);

}
