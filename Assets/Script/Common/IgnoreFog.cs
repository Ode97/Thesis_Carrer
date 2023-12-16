using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreFog : MonoBehaviour
{
    [SerializeField]
    private bool revertFogState = false;
    private Camera camera;
    private void Awake()
    {
        camera = GetComponent<Camera>();
    }
    public void OnPreRender()
    {
        RenderSettings.fog = false;  
    }
    public void OnPostRender()
    {
        Debug.Log("bb");
        RenderSettings.fog = true;
    }

    public void RenderMap()
    {
        
        gameObject.SetActive(true);
        OnPreRender();
        camera.Render();
        OnPostRender();
        gameObject.SetActive(false);
    }
}

