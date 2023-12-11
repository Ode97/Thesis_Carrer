using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointTeleport : MonoBehaviour
{
    public Vector3 pos;
    // Start is called before the first frame update

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(Teleport);
    }

    public void Teleport()
    {
        GameManager.instance.character.transform.position = pos;
    }

    
}
