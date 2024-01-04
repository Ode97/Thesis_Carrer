using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element { None, Air, Fire, Water, Earth }

public abstract class MagicElement : MonoBehaviour
{
    //public Color color;
    public GameObject aurea;
    protected Vector3 objectPosition;
    protected InteractableObject interactableObject;
    public GameObject SpecialAttack;
    public GameObject BaseAttack;
    protected Character character;
    public Element element;

    private void Awake()
    {
        character = GameManager.instance.character;
    }
    public abstract void ApplyEffect();
    
    public void SetObject(InteractableObject obj)
    {
        interactableObject = obj;
    }

    public void SetPosition(Vector3 p)
    {
        objectPosition = p;
    }

    public InteractableObject GetObject()
    {
        return interactableObject;
    }

}
