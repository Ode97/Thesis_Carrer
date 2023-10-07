using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Icon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public MagicElement element;

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.instance.SetElement(element);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }
        
    
}
