using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Diagnostics;

public enum CameraMode {Strategica, Vista}

public class GameManager : MonoBehaviour
{
    public Character character;
    private CameraMode actualMode = CameraMode.Strategica;
    [SerializeField]
    private MainCameraFollow cameraHandler;
    private bool interaction = false;
    private bool justChanged = false;
    private List<InteractableObject> objs = new List<InteractableObject>();
    public GameObject outlineEffect;
    public float fixingTime = 2;
    public bool stopLogic = false;
    public bool fixing = false;
    public Process p;
    

    // Start is called before the first frame update
    public static GameManager instance = null;

    void Awake()
    {
        //p.StartInfo.FileName = "C:\\Users\\Celeste\\Desktop\\EyeTrackerInteraction\\Blank-ADMI\\bin\\Release\\BlankADMI.exe";

        //p.StartInfo.FileName = "D:\\EyeTrackerInteraction\\Blank-ADMI\\bin\\Release\\BlankADMI.exe";
        /*var filePath = "EyeTrackerInteraction\\Blank-ADMI\\bin\\Release\\BlankADMI.exe";
        if (System.IO.File.Exists(filePath))
        {
            p = new Process();
            p.StartInfo.UseShellExecute = true;
            p.StartInfo.FileName = filePath;

            p.Start();
        }*/
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        outlineEffect = Instantiate(outlineEffect);
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
        if (!stopLogic)
        {
            if (character.isActiveElement())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit, Mathf.Infinity);

                
                if (hit.collider && hit.collider.gameObject.layer == Constants.enemyLayer)
                {
                    character.SetEnemy(hit.collider.gameObject);
                }
            }
            //if (!stopLogic && !MenuManager.instance.isMenuOpen()) {
            if (!MenuManager.instance.isMenuOpen())
            {
                if (Input.GetMouseButtonDown(0) || fixing)
                {
                    fixing = false;
                    if (EventSystem.current.IsPointerOverGameObject())
                    {

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

                            if (hit.collider.GetComponent<InteractableObject>())
                            {
                                character.Interaction(hit);
                                return;
                            }
                        }

                        if (actualMode == CameraMode.Strategica)
                        {


                            if (layer == Constants.terrainLayer)
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
            MenuManager.instance.DisableIcon();
            character.StopImmediateAurea();
        }else
            MenuManager.instance.EnableIcon();
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
