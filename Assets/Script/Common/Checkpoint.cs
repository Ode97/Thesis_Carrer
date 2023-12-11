using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI areaName;
    private TextMeshProUGUI areanNameMainCanvas;
    [SerializeField]
    private Canvas main;
    private void OnTriggerEnter(Collider other)
    {
        if(areanNameMainCanvas != null)
        {
            Destroy(areanNameMainCanvas);
        }
        if (other.gameObject.layer == Constants.protagonistLayer)
        {
            GameManager.instance.character.SetCheckpoint(transform.position);
            if (areaName != null)
            {
                areaName.gameObject.SetActive(true);
                areaName.transform.parent.GetComponent<CheckpointTeleport>().pos = transform.position;
                areaName.transform.parent.GetComponent<Button>().enabled = true;

                StartCoroutine(ShowAreaName());
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

    private IEnumerator ShowAreaName()
    {
        
        areanNameMainCanvas = Instantiate(areaName, main.transform);
       
        areanNameMainCanvas.transform.localScale = Vector3.one;
        RectTransform canvasRect = main.transform.GetComponent<RectTransform>();
        Vector2 canvasSize = canvasRect.sizeDelta;

        var v = canvasSize/2;

        v.y += 100;

        areanNameMainCanvas.transform.position = v;
        increaseScale = true;
        yield return new WaitForSeconds(5);

        increaseScale = false;
        Destroy(areanNameMainCanvas);

    }
}
