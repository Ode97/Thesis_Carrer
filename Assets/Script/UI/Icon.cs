using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icon : MonoBehaviour
{
    public MagicElement element;
    public void Element()
    {
        GameManager.instance.SetElement(element);
    }
}
