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
    private Canvas mainCanvas;

    [SerializeField]
    private GameObject icons;

    [SerializeField]
    private GameObject settingsPanel;

    public static MenuManager instance = null;

    private bool menuOpen = false;

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
        menu.SetActive(true);
        menuOpen = true;
    }

    public void CloseMenu()
    {
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
        mainCanvas.enabled = false;
        menuOpen = true;
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
        Debug.Log("A");
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
}
