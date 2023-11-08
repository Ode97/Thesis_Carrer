using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private float timer = 0;
    public GameObject menu;
    private bool start = false;
    public bool activateMainCanvas = false;
    public bool closeGame = false;
    private void Update()
    {
        if (start)
        {
            timer += Time.deltaTime;

            if (timer > 2)
            {
                Reset();
                if (!closeGame)
                    CloseMenu();
                else
                {
                    CloseGame();
                }
                if (activateMainCanvas)
                    Settings.instance.EnableMainCanvas();
            }
        }
    }

    private void CloseMenu()
    {
        
        menu.SetActive(false);
        Settings.instance.SetMenuStatus(false);
        
    }

    private void CloseGame()
    {
        Settings.instance.CloseGame();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        start = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Reset();
    }

    public void Reset()
    {
        start = false;
        timer = 0;
    }
}
