using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class ChangeGameMode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private CameraMode[] modes = new CameraMode[2];
    private int i = 0;
    private TextMeshProUGUI text;
    private bool interaction = false;
    public bool mode = false;
    [SerializeField]
    private RawImage magicImage;
    [SerializeField]
    private RawImage walkingImage;
    [SerializeField]
    private RawImage TDImage;
    [SerializeField]
    private RawImage FPImage;
    // Start is called before the first frame update
    void Start()
    {
        modes[0] = CameraMode.Strategica;
        //modes[1] = GameMode.Interaction;
        modes[1] = CameraMode.Vista;
        //text = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeMode()
    {
        i++;
        if (i == modes.Length) {
            i = 0;
        }

        if (i == 1)
        {
            FPImage.enabled = true;
            TDImage.enabled = false;
        }
        else
        {
            TDImage.enabled = true;
            FPImage.enabled = false;
        }

        GameManager.instance.SetMode(modes[i]);
        //text.text = modes[i].ToString();
    }

    public void InteractionMode()
    {
        interaction = !interaction;
        if (interaction)
        {
            magicImage.enabled = true;
            walkingImage.enabled = false;
            //text.text = "Magia attiva";
        }
        else
        {
            magicImage.enabled = false;
            walkingImage.enabled = true;
        }
        //text.text = "Magia disattivata";

        GameManager.instance.SetInteraction(interaction);
        
    }

    public static bool click = false;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!click)
        {
            if (mode)
                ChangeMode();
            else
                InteractionMode();
        }
    }

    

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (click)          
            if (mode)
                ChangeMode();
            else
                InteractionMode();
    }
}
