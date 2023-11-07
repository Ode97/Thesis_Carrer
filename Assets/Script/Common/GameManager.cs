using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
//using static UnityEditor.Experimental.GraphView.GraphView;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using System.Diagnostics;

public enum CameraMode { Strategica, Vista}

public class GameManager : MonoBehaviour
{
    public Character character;
    private CameraMode actualMode = CameraMode.Strategica;
    [SerializeField]
    private MainCameraFollow cameraHandler;
    private bool interaction = false;
    private bool justChanged = false;
    private List<InteractableObject> objs = new List<InteractableObject>();
    public bool stopLogic = false;
    public bool fixing = false;
    public Process p;
    

    // Start is called before the first frame update
    public static GameManager instance = null;

    void Awake()
    {
        p = new Process();
        p.StartInfo.UseShellExecute = true;
        //p.StartInfo.FileName = "C:\\Users\\Celeste\\Desktop\\EyeTrackerInteraction\\Blank-ADMI\\bin\\Release\\BlankADMI.exe";
        //p.StartInfo.FileName = "E:\\EyeTrackerInteraction\\Blank-ADMI\\bin\\Release\\BlankADMI.exe";
        //p.StartInfo.FileName = "D:\\EyeTrackerInteraction\\Blank-ADMI\\bin\\Release\\BlankADMI.exe";
        //p.Start();

        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }


        // Start is called before the first frame update
    void Start()
    {
        var t = FindObjectOfType<Terrain>();
        SetObject(t.GetComponent<InteractableObject>());
    }

    // Update is called once per frame
    void Update()
    {
        if (character.isActiveElement())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Enemy"))) {
                
                character.SetEnemy(hit.collider.GetComponentInParent<Enemy>());
            }
        }

        if ((Input.GetMouseButtonDown(0) && !stopLogic) || (fixing && !stopLogic))
        {
            fixing = false;
            if (EventSystem.current.IsPointerOverGameObject()) {
                
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, Mathf.Infinity);

            // Cast a ray from the mouse position into the scene
            if (hit.collider != null)
            {
                
                var layer = hit.collider.gameObject.layer;
                
                if (interaction)
                {
                   
                    if (layer == LayerMask.NameToLayer("Terrain") || layer == LayerMask.NameToLayer("InteractableObject"))
                    {
                        character.Interaction(hit);
                        return;
                    }
                }

                if (actualMode == CameraMode.Strategica)
                {
                   

                    if (layer == LayerMask.NameToLayer("Terrain"))
                    {

                        character.MoveToDestination(hit.point);
                        return;
                    }

                }
                else
                {

                }
            }
        }
    }

    public void SetObject(InteractableObject obj)
    {
        if (!stopLogic) 
        { 
            //Debug.Log(obj.name);
            if (!justChanged )
            {
                //Debug.Log("a" + obj.name);

                justChanged = true;
                character.SetObject(obj);
                //StartCoroutine(EndSelect());
            }
            else if(!objs.Contains(obj))
                objs.Add(obj);
        }else
            objs.Add(obj);

        return;
    }

    public void SetInteraction(bool interaction)
    {
        this.interaction = interaction;
        if (!interaction)
        {
            Settings.instance.DisableIcon();
            character.StopImmediateAurea();
        }else
            Settings.instance.EnableIcon();
    }

    public bool IsInteraction()
    {
        return interaction;
    }

    /*private IEnumerator EndSelect()
    {
        yield return new WaitForSeconds(0.3f);
        EndSelection();
    }*/

    public void EndSelection()
    {
        if (objs.Count > 0)
        {
            character.SetObject(objs[objs.Count - 1]);
            objs.Clear();            
        }
        else 
            justChanged = false;
    }


    public void SetElement(MagicElement element)
    {
        if(interaction)
            character.SetActualElement(element);
    }

    public void SetMode(CameraMode mode)
    {
        actualMode = mode;
        cameraHandler.SetMode(mode);
    }
}
