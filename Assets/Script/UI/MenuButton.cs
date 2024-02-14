using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerUpHandler
{
    private float timer = 0;
    public GameObject menu;
    public DialogueManager dialogueManager;
    public bool dialogue = false;
    private bool start = false;
    public bool activateMainCanvas = false;
    public bool activateStartCanvas = false;
    public bool closeGame = false;
    public bool newGame = false;
    public bool continueGame = false;
    public bool save = false;
    public bool plant = false;
    public bool restart = false;
    public bool map = false;
    public bool changeCursorModality = false;
    public bool changeInteractionModality = false;
    public bool chaingMainButton = false;
    public bool changingMagicModality = false;
    public bool timerButton = false;
    public bool increaseAtt = false;
    public bool decreaseAtt = false;
    public bool teleport = false;

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    private void Update()
    {
        if (start)
        {
            timer += Time.unscaledDeltaTime;
            GameManager.instance.SetSliderTime(timer);
            if (timer > GameManager.instance.fixingTime)
            {
                Reset();                

                if (dialogue)
                    DialogueContinue();

                if (plant)
                    Chosen();

                if (newGame)
                {
                    NewGame();
                }
                if (continueGame)
                {
                    ContinueGame();
                }
                if (save)
                {
                    GameManager.instance.Save();
                }
                if (activateMainCanvas)
                {
                    MenuManager.instance.EnableMainCanvas();
                }
                if (activateStartCanvas)
                {
                    dialogueManager.EndDialogue();
                    MenuManager.instance.OpenMenuStart();

                }
                if (changeCursorModality)
                {
                    GameSettings.instance.ChangeCursor();
                }
                if (changeInteractionModality)
                {
                    GameSettings.instance.ChangeInteraction();
                }
                if (chaingMainButton)
                {
                    GameSettings.instance.ChangeCam();
                }
                if (changingMagicModality)
                {
                    GameSettings.instance.ChangeMagic();
                }
                if(decreaseAtt)
                    GameSettings.instance.DecreaseAttentionTime();

                if(increaseAtt)
                    GameSettings.instance.IncreseAttentionTime();

                if (teleport)
                {
                    GetComponent<CheckpointTeleport>().Teleport();
                }

                if (menu)
                {
                    
                    if (menu.activeSelf)
                    {
                        CloseMenu();
                    }
                    else
                        OpenMenu();
                }

                if (closeGame)
                {
                    CloseGame();
                }

            }
        }
    }
    
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
        GetComponent<DialogueTrigger>().TriggerDialogue();
        GameManager.instance.character.GameStart();
        MenuManager.instance.StartNewGame();
        
    }

    private void ContinueGame()
    {
        MenuManager.instance.StartNewGame();
        DataManager.loadData();
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
        GameManager.instance.SetSliderTime(0);
        GameManager.instance.selectionSlider.SetActive(true);
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        GameManager.instance.selectionSlider.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Reset();
        GameManager.instance.selectionSlider.SetActive(false);
    }

    public void Reset()
    {
        
        start = false;
        timer = 0;
        GameManager.instance.SetSliderTime(0);
    }

    public void DialogueContinue()
    {
        dialogueManager.DisplayNext();
    }

    public void Chosen()
    {
        GetComponentInChildren<EarthPlant>().Chosen();
    }

    public void IncreaseAttentionTime()
    {
        GameSettings.instance.IncreseAttentionTime();
    }

    public void DecreaseAttentionTime()
    {
        GameSettings.instance.DecreaseAttentionTime();
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        Reset();

        if (dialogue)
            DialogueContinue();

        if (plant)
            Chosen();

        if (newGame)
        {
            NewGame();
        }
        if (continueGame)
        {
            ContinueGame();
        }
        if (save)
        {
            GameManager.instance.Save();
        }
        if (activateMainCanvas)
        {
            MenuManager.instance.EnableMainCanvas();
        }
        if (activateStartCanvas)
        {
            dialogueManager.EndDialogue();
            MenuManager.instance.OpenMenuStart();

        }
        if (changeCursorModality)
        {
            GameSettings.instance.ChangeCursor();
        }
        if (changeInteractionModality)
        {
            GameSettings.instance.ChangeInteraction();
        }
        if (chaingMainButton)
        {
            GameSettings.instance.ChangeCam();
        }
        if (changingMagicModality)
        {
            GameSettings.instance.ChangeMagic();
        }
        if (decreaseAtt)
            GameSettings.instance.DecreaseAttentionTime();

        if (increaseAtt)
            GameSettings.instance.IncreseAttentionTime();


        if (menu)
        {
            if (menu.activeSelf)
            {
                CloseMenu();
            }
            else
            {
                OpenMenu();
            }
        }

        if (closeGame)
        {
            CloseGame();
        }



    }
}
