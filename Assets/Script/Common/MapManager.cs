using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> mapsName = new List<TextMeshProUGUI>();

    private static MapManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

        // Update is called once per frame
    void Update()
    {
        
    }
}
