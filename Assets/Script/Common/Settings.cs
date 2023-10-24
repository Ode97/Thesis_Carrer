using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private GameObject earthPanel;

    [SerializeField]
    private Canvas mainCanvas;

    [SerializeField]
    private GameObject icons;

    public static Settings instance = null;

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
            OpenSettings();
    }

    public void OpenSettings()
    {
        menu.SetActive(true);
    }

    public void CloseSettings()
    {
        menu.SetActive(false);
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void OpenEarthMenu()
    {
        earthPanel.SetActive(true);
        mainCanvas.enabled = false;
    }

    public void CloseEarthMenu()
    {
        earthPanel.SetActive(false);
        mainCanvas.enabled = true;
    }

    public void EnableIcon()
    {
        icons.SetActive(true);
    }

    public void DisableIcon()
    {
        icons.SetActive(false);
    }

}
