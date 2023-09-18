using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element { None, Air, Fire, Water, Earth }

public class MagicElement : MonoBehaviour
{
    public Element element;
    public Color color;
    public GameObject aurea;
    public Vector3 pos;

}
