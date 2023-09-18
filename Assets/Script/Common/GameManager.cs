using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public enum GameMode { Movment, View, Interaction}

public class GameManager : MonoBehaviour
{
    public Character character;
    public GameMode actualMode = GameMode.Movment;
    

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

            if (actualMode == GameMode.Movment)
            {
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("InteractableObject")))
                {
                    return;
                }
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Terrain")))
                {

                    character.MoveToDestination(hit.point);
                    return;
                }
            }
            else if (actualMode == GameMode.Interaction)
            {

                string[] layerNames = new string[] {"Terrain",  "InteractableObject"};
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask(layerNames)))
                {
                    hit.collider.gameObject.GetComponent<InteractableObject>().Interaction(hit.point);
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
    }

    
}
