using System.Collections;
using System.Collections.Generic;
using System.Configuration.Assemblies;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Unity.VisualScripting;

public class GameSettings : MonoBehaviour
{
    [SerializeField]
    private Button cursorButton;
    [SerializeField]
    private Button interactionButton;
    [SerializeField]
    private Button camButton;
    [SerializeField]
    private Button magicButton;
    [SerializeField]
    private Slider attentionTime;

    private string[] cursor = new string[2];
    private string[] interaction = new string[3];
    private string[] cam = new string[3];
    private string[] magic = new string[3];

    public static GameSettings instance = null;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        cursor[0] = "Mouse";
        cursor[1] = "Eye Tracker";

        interaction[0] = "Click del Mouse";
        interaction[1] = "Fissare o Occhiolino";
        interaction[2] = "Microfono";

        cam[0] = "Click del Mouse";
        cam[1] = "Fissare o Occhiolino";
        cam[2] = "Spostamento del cursore";       

        magic[0] = "Click del Mouse";
        magic[1] = "Fissare o Occhiolino";
        magic[2] = "Spostamento del cursore";

        cursorButton.GetComponentInChildren<TextMeshProUGUI>().text = cursor[i];
        interactionButton.GetComponentInChildren<TextMeshProUGUI>().text = interaction[i];
        camButton.GetComponentInChildren<TextMeshProUGUI>().text = cam[i];
        magicButton.GetComponentInChildren<TextMeshProUGUI>().text = magic[i];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private int i = 0;
    public void ChangeCursor()
    {
        i++;
        if (i >= cursor.Length)
            i = 0;

        cursorButton.GetComponentInChildren<TextMeshProUGUI>().text = cursor[i];

        if (i == 0)
        {
            if (j == 1)
            {
                j++;
                interactionButton.GetComponentInChildren<TextMeshProUGUI>().text = interaction[j];
            }

            if(k == 1)
            {
                k++;
                camButton.GetComponentInChildren<TextMeshProUGUI>().text = cam[k];
            }

            if (l == 1)
            {
                l++;
                magicButton.GetComponentInChildren<TextMeshProUGUI>().text = magic[l];
            }

            GameManager.instance.p.Kill();
            //Application.Quit();
        }
        else
        {
            if (j == 0)
            {
                j++;
                interactionButton.GetComponentInChildren<TextMeshProUGUI>().text = interaction[j];
            }

            if (k == 0)
            {
                k++;
                camButton.GetComponentInChildren<TextMeshProUGUI>().text = cam[k];
            }

            if (l == 0)
            {
                l++;
                magicButton.GetComponentInChildren<TextMeshProUGUI>().text = magic[l];
            }

            /*string appFolderPath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
            UnityEngine.Debug.Log(appFolderPath);
            string exeFolder = Path.GetDirectoryName(appFolderPath);
            UnityEngine.Debug.Log(exeFolder);*/
            // Specifica il nome della cartella da aprire
            string folderToOpen = "EyeTrackerInteraction\\Blank-ADMI\\bin\\Release\\BlankADMI";

            // Unisci i percorsi per ottenere il percorso completo della cartella da aprire
            //string folderPath = Path.Combine(exeFolder, folderToOpen);

            //UnityEngine.Debug.Log(folderPath);

            // Il percorso completo del file .txt che desideri scrivere
            string filePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, folderToOpen);
            
            //string writePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Output.txt");
            //string aPath = Path.Combine("C:\\Users\\edoma\\Desktop", "Output1.txt");

            //string content = filePath.ToString();

            /*using (StreamWriter writer = new StreamWriter(aPath))
            {
                writer.Write(writePath);
            }

            // Scrivi il contenuto nel file
            using (StreamWriter writer = new StreamWriter(writePath))
            {
                writer.Write(content);
            }*/

            if (System.IO.File.Exists(filePath))
            {
                GameManager.instance.p = new Process();
                GameManager.instance.p.StartInfo.UseShellExecute = true;
                GameManager.instance.p.StartInfo.FileName = filePath;

                GameManager.instance.p.Start();
            }
        }
    }

    private int j = 0;
    public void ChangeInteraction()
    {

        j++;

        if (j >= interaction.Length)
            j = 0;

        if (j == 1)
        {
            if(i == 0)
                j++;

            InteractableObject.click = false;
        }

        if (j == 0)
        {
            if(i == 1)
                j++;

            InteractableObject.click = true;
        }

        interactionButton.GetComponentInChildren<TextMeshProUGUI>().text = interaction[j];
    }

    private int k = 0;
    public void ChangeCam()
    {
        k++;
        if (k >= cam.Length)
            k = 0;

        if (k == 1)
        {
            if(i == 0)
                k++;
            else
                ChangeGameMode.click = false;
        }

        if (k == 0)
        {
            if (i == 1)
            {
                k++;
            }else
                ChangeGameMode.click = true;

        }

        if(k == 2)
            ChangeGameMode.click = false;

        camButton.GetComponentInChildren<TextMeshProUGUI>().text = cam[k];

    }

    private int l = 0;
    public void ChangeMagic()
    {
        l++;
        if (l >= magic.Length)
            l = 0;

        if (l == 1)
        {
            if(i == 0)
                l++;
            else
                Icon.click = false;
        }

        if (l == 0)
        {
            if(i == 1)
                l++;
            else
                Icon.click = true;
        }

        if(l == 3)
        {
            Icon.click = false;
        }

        magicButton.GetComponentInChildren<TextMeshProUGUI>().text = magic[l];
    }

    public void IncreseAttentionTime()
    {
        if(attentionTime.value < 4)
            attentionTime.value += 0.2f;

        attentionTime.GetComponentInChildren<TextMeshProUGUI>().text = attentionTime.value.ToString();

        GameManager.instance.fixingTime = attentionTime.value;
    }

    public void DecreaseAttentionTime()
    {
        if(attentionTime.value > 0)
            attentionTime.value -= 0.2f;

        attentionTime.GetComponentInChildren<TextMeshProUGUI>().text = attentionTime.value.ToString();

        GameManager.instance.fixingTime = attentionTime.value;
    }
}
