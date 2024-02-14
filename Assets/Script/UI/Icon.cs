using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Icon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public MagicElement element;
    private float t = 0;
    private bool start = false;
    public static bool click = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!click)
        {
            GameManager.instance.SetElement(element);


            if (element.GetComponent<EarthElement>())
            {
                start = true;
                GameManager.instance.SetSliderTime(0);
                GameManager.instance.selectionSlider.SetActive(true);
            }
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        t = 0;
        start = false;
        GameManager.instance.SetSliderTime(0);
        GameManager.instance.selectionSlider.SetActive(false);
    }

    private void Update()
    {
        if(start)
        {
            
            t += Time.deltaTime;

            GameManager.instance.SetSliderTime(t);
            if (t >= GameManager.instance.fixingTime)
            {
                MenuManager.instance.OpenEarthMenu();
                
            }
        }

        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (click)
        {
            
            GameManager.instance.SetElement(element);
            if (element.GetComponent<EarthElement>())
                MenuManager.instance.OpenEarthMenu();
            
        }
    }
}
