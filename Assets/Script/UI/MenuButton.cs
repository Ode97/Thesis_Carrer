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
    public bool activateStartCanvas = false;
    public bool closeGame = false;
    public bool newGame = false;
    public bool continueGame = false;
    public bool save = false;

    private void Update()
    {
        if (start)
        {          
            timer += Time.unscaledDeltaTime;
            if (timer > GameManager.instance.fixingTime)
            {
                Reset();

                if (newGame)
                {
                    NewGame();
                }else if (continueGame)
                {
                    ContinueGame();
                }else if (save)
                {
                    GameManager.instance.saveCity.SaveState();
                }
                else
                {

                    if (!closeGame)
                    {
                        if (menu)
                        {
                            if (menu.activeSelf)
                                CloseMenu();
                            else
                                OpenMenu();
                        }
                    }
                    else
                    {
                        CloseGame();
                    }
                }
                if (activateMainCanvas)
                    MenuManager.instance.EnableMainCanvas();
                else if (activateStartCanvas)
                {
                    Debug.Log("a");
                    MenuManager.instance.OpenMenuStart();
                }
            }
        }
    }

    public bool restart = false;
    public bool map = false;
    private void CloseMenu()
    {
        Reset();

        if (restart)
        {
            Time.timeScale = 1f;
            MenuManager.instance.SetMenuStatus(false);
        }
        menu.SetActive(false);
                             
        if(map)
            CloseMap();

    }

    private void OpenMenu()
    {
        Reset();
        menu.SetActive(true);
        if(map)
            OpenMap();
    }

    private void NewGame()
    {
        MenuManager.instance.StartNewGame();
    }

    private void ContinueGame()
    {
        MenuManager.instance.StartNewGame();
        Save.loadData();
    }

    private void CloseGame()
    {
        MenuManager.instance.CloseGame();
    }

    private void OpenMap()
    {
        MenuManager.instance.OpenMap();
    }

    private void CloseMap()
    {
        MenuManager.instance.OpenMap();
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

        if (newGame)
        {
            NewGame();
        }
        else if (continueGame)
        {
            ContinueGame();
        }
        else if (save)
        {
            GameManager.instance.saveCity.SaveState();
        }
        else
        {
            if (menu)
            {


                if (!menu.activeSelf)
                    OpenMenu();
                else
                    CloseMenu();
            }
        }

        if (activateMainCanvas)
            MenuManager.instance.EnableMainCanvas();
        else if (activateStartCanvas)
        {
            MenuManager.instance.OpenMenuStart();
        }
    }
}
