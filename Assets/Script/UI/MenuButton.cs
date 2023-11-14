using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
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

            if (timer > GameManager.instance.fixingTime)
            {
                Reset();
                if (!closeGame)
                {
                    if (menu.activeSelf)
                        CloseMenu();
                    else
                        OpenMenu();
                }
                else
                {
                    CloseGame();
                }
                if (activateMainCanvas)
                    MenuManager.instance.EnableMainCanvas();
            }
        }
    }

    public bool restart = false;
    private void CloseMenu()
    {
        Reset();
        menu.SetActive(false);
        
        if(restart)
            Time.timeScale = 1f;
        
        
    }

    private void OpenMenu()
    {
        Reset();
        menu.SetActive(true);
    }

    private void CloseGame()
    {
        MenuManager.instance.CloseGame();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        start = true;
        timer = 0;
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

    
    public void OnPointerClick(PointerEventData eventData)
    {
        Reset();

        if (activateMainCanvas)
            MenuManager.instance.EnableMainCanvas();

        if (!menu.activeSelf)
            OpenMenu();
        else
            CloseMenu();
    }
}
