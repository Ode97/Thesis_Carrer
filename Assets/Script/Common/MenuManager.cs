using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject menu;

    [SerializeField]
    private GameObject startMenu;

    [SerializeField]
    private GameObject earthPanel;

    [SerializeField]
    private GameObject mapCanvas;

    [SerializeField]
    private Canvas mainCanvas;

    [SerializeField]
    private GameObject icons;

    [SerializeField]
    private GameObject settingsPanel;

    public static MenuManager instance = null;

    private bool menuOpen = true;

    [SerializeField]
    private GameObject mainButtons;

    [SerializeField]
    private GameObject mapIcon;

    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Camera mapCamera;

    private IgnoreFog renderCam;

    private bool start = false;
    private bool firstStart = false;

    private Vector3 initCameraPos;
    private Quaternion initCameraRot;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        renderCam = mapCamera.GetComponent<IgnoreFog>();
    }

    private void Start()
    {
        initCameraPos = Camera.main.transform.position;
        initCameraRot = Camera.main.transform.rotation;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(2) && start)
            OpenMenu();
    }

    public void OpenMenu()
    {
        //Time.timeScale = 0f;
        menu.SetActive(true);
        menuOpen = true;
    }

    public void CloseMenu()
    {
        Time.timeScale = 1f;
        menu.SetActive(false);
        menuOpen = false;
    }

    public void OpenMenuStart()
    {
        startMenu.SetActive(true);
        mainButtons.SetActive(false);
        GameManager.instance.ResetGame();
        Camera.main.GetComponent<MainCameraFollow>().enabled = false;
        Camera.main.transform.position = initCameraPos;
        Camera.main.transform.rotation = initCameraRot;
        AudioManager.instance.PlayForestMusic();
        menuOpen = false;
        start = false;
        Time.timeScale = 1f;
    }

    public void CloseGame()
    {
        GameManager.instance.csvBuilder.EndCSV();
        try 
        {
            GameManager.instance.p.Kill();
        }
        catch 
        {
            //Application.Quit();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        System.Diagnostics.Process.GetCurrentProcess().Kill();
    }

    public void OpenEarthMenu()
    {
        earthPanel.SetActive(true);
        mainCanvas.enabled = false;
        menuOpen = true;
    }

    public void OpenMap()
    {
        mapCanvas.SetActive(true);
        var p = GameManager.instance.character.transform.position;
        mapIcon.transform.position = new Vector3(p.x, mapIcon.transform.position.y, p.z);
        
        renderCam.RenderMap();


    }
    public void CloseMap()
    {

        mapCanvas.SetActive(false);
        mainCanvas.enabled = true;
        mapCamera.gameObject.SetActive(false);
        //RenderSettings.fog = true;
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
        //startMenu.gameObject.SetActive(false);
        GameManager.instance.outlineEffect.SetActive(true);
        GameManager.instance.stopLogic = false;
        mainCamera.GetComponent<MainCameraFollow>().enabled = true;
        //mainCanvas.gameObject.SetActive(true);
        mainButtons.SetActive(true);
        firstStart = true;
        menuOpen = false;
        start = true;
        
    }

    public bool IsFirstStart()
    {
        return firstStart;
    }
}
