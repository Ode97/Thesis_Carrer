using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Icon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public MagicElement element;
    private float t = 0;
    private bool start = false;


    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.instance.SetElement(element);
        if(element.GetComponent<EarthElement>())
            start = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        t = 0;
        start = false;
    }

    private void Update()
    {
        if(start)
        {
            t += Time.deltaTime;
            if (t >= 2)
            {
                Settings.instance.OpenEarthMenu();
                
            }
        }
    }
}
