using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public enum GameMode { Movment, View, Interaction}

public class GameManager : MonoBehaviour
{
    public Character character;
    public GameMode actualMode = GameMode.Movment;
    [SerializeField]
    public MainCameraFollow camera_mode;
    

    // Start is called before the first frame update
    public static GameManager instance = null;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) {
                return;
            }

            // Cast a ray from the mouse position into the scene
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            string[] layerNames = new string[] { "Terrain", "InteractableObject" };
            Physics.Raycast(ray, out hit, Mathf.Infinity);
            
            var layer = hit.collider.gameObject.layer;
            if (actualMode == GameMode.Movment)
            {
                if (layer == LayerMask.NameToLayer("Terrain"))
                {
                    

                    character.MoveToDestination(hit.point);
                    return;
                }
            }
            else if (actualMode == GameMode.Interaction)
            {

                if (layer == LayerMask.NameToLayer("Terrain") || layer == LayerMask.NameToLayer("InteractableObject"))
                {
                    
                    character.Interaction(hit);
                    
                    return;
                }
            }
        }
    }

    public void SetElement(MagicElement element)
    {
        character.SetActualElement(element);
    }

    public void SetMode(GameMode mode)
    {
        actualMode = mode;
        camera_mode.SetMode(mode);
    }

    
}
