using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI areaName;
    private TextMeshProUGUI areanNameMainCanvas;
    [SerializeField]
    private GameObject areaPanel;
    private Save sc;
    [SerializeField]
    private int index;
    private bool discovered = false;

    private void Awake()
    {
        sc = FindObjectOfType<Save>();
        EventManager.StartListening("Reset", Reset);
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if(areanNameMainCanvas != null)
        {            
            Destroy(areanNameMainCanvas);
        }*/

        if (other.gameObject.layer == Constants.protagonistLayer && GameManager.instance.character.IsGameStart())
        {
            if (!AudioManager.instance.forest.isPlaying)
                AudioManager.instance.PlayForestMusic();

            sc.SaveState();
            if (!discovered)
            {
                GameManager.instance.character.SetCheckpoint(this);

                if (areaName != null)
                {
                    areaName.gameObject.SetActive(true);
                    areaName.transform.parent.GetComponent<CheckpointTeleport>().pos = transform.position;
                    areaName.transform.parent.GetComponent<Button>().enabled = true;

                    if (!show)
                        StartCoroutine(ShowAreaName());


                }
                GetComponent<DialogueTrigger>()?.TriggerDialogue();
                discovered = true;
            }
        }
        
    }

    private bool increaseScale = false;
    private void Update()
    {
        if(increaseScale && areanNameMainCanvas.transform.localScale.x < 5)
        {
            areanNameMainCanvas.transform.localScale += Vector3.one * 0.1f;
        }
    }

    private bool show = false;
    private IEnumerator ShowAreaName()
    {
        if (areanNameMainCanvas == null)
        {
            areanNameMainCanvas = Instantiate(areaName, areaPanel.transform);
            areanNameMainCanvas.transform.localScale = Vector3.one;
            //RectTransform canvasRect = main.transform.GetComponent<RectTransform>();
            //Vector2 canvasSize = canvasRect.sizeDelta;

            Vector2 screenSize = new Vector2(Screen.width, Screen.height);

            var v = screenSize / 2;

            v.y += 300;

            areanNameMainCanvas.transform.position = v;
            increaseScale = true;
            show = true;
            yield return new WaitForSeconds(3);
            
            increaseScale = false;
            Destroy(areanNameMainCanvas.gameObject);
            show = false;
            
            
        }
        
    }

    public bool IsDiscovered()
    {
        return discovered;
    }

    public void SetDiscovered(bool d)
    {
        discovered = d;
        if (areaName && discovered) {
            areaName.gameObject.SetActive(true);
            areaName.transform.parent.GetComponent<CheckpointTeleport>().pos = transform.position;
            areaName.transform.parent.gameObject.SetActive(true);
            areaName.transform.parent.GetComponent<Button>().enabled = true;
        }
    }

    private void Reset()
    {
        discovered = false;
        if (areaName)
        {
            areaName.gameObject.SetActive(false);
            areaName.transform.parent.GetComponent<Button>().enabled = false;
        }
        
    }

    public int GetIndex()
    {
        return index;
    }
}
