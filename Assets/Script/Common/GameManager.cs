using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Diagnostics;
using Unity.VisualScripting;

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
    public InteractableObject initTerrain;
    

    // Start is called before the first frame update
    public static GameManager instance = null;

    void Awake()
    {
        
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        outlineEffect = Instantiate(outlineEffect);
        outlineEffect.SetActive(false);
        SetObject(initTerrain);
        stopLogic = true;
    }


        // Start is called before the first frame update
    void Start()
    {
        
        
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
                            //cambiare con layer e vedere se si rompe tutto
                            if (hit.collider.GetComponent<InteractableObject>())
                            {
                                character.Interaction(hit);
                                
                            }
                            return;
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
            if (!justChanged)
            {
                justChanged = true;
                
                character.SetObject(obj);
                //StartCoroutine(EndSelect());
            }
            else if (!objs.Contains(obj))
            {
                objs.Add(obj);
            }
        }
        else
        {
            
            objs.Add(obj);
        }

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
            //UnityEngine.Debug.Log(objs.Count);
            //UnityEngine.Debug.Log(objs[objs.Count - 1].name);
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

    public void SetLoad(int d, float[] pPos, float[] pRot, float[,,] pE, float[,,] rE, float[,] aP, float[,] aR, int[] fD, bool[] wD)
    {
        character.SetDiamonds(d);

        character.transform.position = new Vector3(pPos[0], pPos[1], pPos[2]);
        character.transform.rotation = new Quaternion(pRot[0], pRot[1], pRot[2], 1);

        var x = FindObjectsOfType<Air>();
        var i = 0;
        foreach (Air a in x)
        { 
            a.gameObject.transform.position = new Vector3(aP[i, 0], aP[i, 1], aP[i, 2]);
            a.gameObject.transform.rotation = new Quaternion(aR[i, 0], aR[i, 1], aR[i, 2], 1);
            i++;
        }

        var y = FindObjectsOfType<Fire>();
        i = 0;
        foreach (Fire f in y)
        {
            if (fD[i] == 1)
            {
                f.FireInteraction();
            }
            i++;
        }

        var z = FindObjectsOfType<WaterObj>();
        i = 0;
        foreach (WaterObj w in z)
        {
            if (wD[i])
            {
                w.WaterInteraction();
            }
            i++;
        }


    }
}
