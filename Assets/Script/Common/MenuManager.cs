using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private GameObject earthPanel;

    [SerializeField]
    private GameObject mapCanvas;

    [SerializeField]
    private Canvas mainCanvas;
    [SerializeField]
    private Canvas startMenuCanvas;

    [SerializeField]
    private GameObject icons;

    [SerializeField]
    private GameObject settingsPanel;

    public static MenuManager instance = null;

    private bool menuOpen = false;

    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Camera menuCamera;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(2))
            OpenMenu();
    }

    public void OpenMenu()
    {
        Time.timeScale = 0f;
        menu.SetActive(true);
        menuOpen = true;
    }

    public void CloseMenu()
    {
        Time.timeScale = 1f;
        menu.SetActive(false);
        menuOpen = false;
    }

    public void CloseGame()
    {
        GameManager.instance.p.Kill();
        Application.Quit();
    }

    public void OpenEarthMenu()
    {
        earthPanel.SetActive(true);
        //mainCanvas.enabled = false;
        menuOpen = true;
    }

    public void OpenMap()
    {
        mapCanvas.SetActive(true);
        //mainCanvas.enabled = false;
        Debug.Log("aa");
        //menuOpen = true;
        
    }
    public void CloseMap()
    {

        mapCanvas.SetActive(false);
        mainCanvas.enabled = true;
        //menuOpen = false;
        
    }


    public void CloseEarthMenu()
    {
        
        earthPanel.SetActive(false);
        mainCanvas.enabled = true;
        menuOpen = false;
    }

    public void EnableIcon()
    {
        icons.SetActive(true);
    }

    public void DisableIcon()
    {
        icons.SetActive(false);
    }

    public void EnableMainCanvas()
    {
        mainCanvas.enabled = true;
    }

    public bool isMenuOpen()
    {
        return menuOpen;
    }

    public void SetMenuStatus(bool status)
    {
        menuOpen = status;
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void StartNewGame()
    {
        menuCamera.gameObject.SetActive(false);
        startMenuCanvas.gameObject.SetActive(false);
    }

    public void ContinueGame()
    {

    }
}
