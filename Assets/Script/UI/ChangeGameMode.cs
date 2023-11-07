using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChangeGameMode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private CameraMode[] modes = new CameraMode[2];
    private int i = 0;
    private TextMeshProUGUI text;
    private bool interaction = false;
    public bool mode = false;
    // Start is called before the first frame update
    void Start()
    {
        modes[0] = CameraMode.Strategica;
        //modes[1] = GameMode.Interaction;
        modes[1] = CameraMode.Vista;
        text = GetComponentInChildren<TextMeshProUGUI>();
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
        GameManager.instance.SetMode(modes[i]);
        text.text = modes[i].ToString();
    }

    public void InteractionMode()
    {
        interaction = !interaction;
        if (interaction)
            text.text = "Magia attiva";
        else
            text.text = "Magia disattiva";

        GameManager.instance.SetInteraction(interaction);
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(mode)
            ChangeMode();
        else
            InteractionMode();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }
}
