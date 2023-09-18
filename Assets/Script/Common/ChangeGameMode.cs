using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChangeGameMode : MonoBehaviour
{
    private GameMode[] modes = new GameMode[3];
    private int i = 0;
    private TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        modes[0] = GameMode.Movment;
        modes[1] = GameMode.Interaction;
        modes[2] = GameMode.View;
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = modes[i].ToString();
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
}
