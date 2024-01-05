using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Diagnostics;
using Unity.VisualScripting;
using System.Linq;
using System;

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
    private Save save;
    public LayerMask ignoreLayer;


    // Start is called before the first frame update
    public static GameManager instance = null;


    private CreateCSV csvBuilder = new CreateCSV();

    public void Save()
    {
        save.SaveState();
    }

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
        
        save = GetComponent<Save>();
    }


    // Update is called once per frame
    void Update()
    {
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, Mathf.Infinity);
        long timestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
        csvBuilder.AddData(timestamp + ";" + actualMode.ToString() + ";" + character.GetObject() + ";" + character.getActualElement() + ";" + character.IsMoving() + ";" + character.IsAttacking() + ";" + Input.mousePosition.x + ";" + Input.mousePosition.y);
        //UnityEngine.Debug.Log(stopLogic + " " + MenuManager.instance.isMenuOpen());
        if (!stopLogic)
        {
            if (character.isActiveElement())
            {
                //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //RaycastHit hit;
                Physics.Raycast(ray, out hit, Mathf.Infinity, ~ignoreLayer);

                
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

                    //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    //RaycastHit hit;

                    Physics.Raycast(ray, out hit, Mathf.Infinity, ~ignoreLayer);

                    //UnityEngine.Debug.Log(hit.collider.gameObject.layer);
                    // Cast a ray from the mouse position into the scene
                    if (hit.collider != null)
                    {
                       
                        
                        var layer = hit.collider.gameObject.layer;

                        if (interaction)
                        {
                            var intObj = hit.collider.GetComponent<InteractableObject>();
                            
                            intObj.ResetTimer();
                            //cambiare con layer e vedere se si rompe tutto
                            if (intObj)
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

    public void SetLoad(int d, float[] pPos, float[] pRot, float[,] pE, float[,] rE, float[,] aP, float[,] aR, int[] fD, bool[] wD, int cP, bool[,] enigmasComplete)
    {
        character.SetDiamonds(d);

        character.transform.position = new Vector3(pPos[0], pPos[1], pPos[2]);
        character.transform.rotation = new Quaternion(pRot[0], pRot[1], pRot[2], pRot[3]);

        var x = FindObjectsOfType<Air>();
        var i = 0;
        foreach (Air a in x)
        { 
            a.gameObject.transform.position = new Vector3(aP[i, 0], aP[i, 1], aP[i, 2]);
            a.gameObject.transform.rotation = new Quaternion(aR[i, 0], aR[i, 1], aR[i, 2], 1);
            i++;
        }

        var fires = FindObjectsOfType<Fire>();
        //Fire[] filteredArray = fires.Where(obj => obj.GetComponent<EnigmaObj>() == null).ToArray();
        var z = fires.OrderBy(fire => fire.GetComponent<EnigmaObj>()?.value).ToArray();
        

        i = 0;
        foreach (Fire f in z)
        {
            if (fD[i] == 1)
            {
                
                if (f.distructible)
                {
                    //UnityEngine.Debug.Log("destroy " + f.name);
                    f.LoadDestroy();
                }
                else
                {
                    //UnityEngine.Debug.Log("open " + f.name);
                    f.FireInteraction();
                }
            }
            i++;
        }



        if (MenuManager.instance.IsFirstStart())
        {
            var p = FindObjectsOfType<WaterObj>();
            i = 0;
            foreach (WaterObj w in p)
            {
                if (wD[i])
                {
                    w.WaterInteraction();
                }
                i++;
            }
        }
        
        var r = FindObjectsOfType<Checkpoint>();      

        foreach (Checkpoint c in r)
        {
            if(c.GetIndex() == cP)
            {
                character.SetCheckpoint(c);
            }
        }

        var t = FindObjectsOfType<EarthPlant>(includeInactive:true);
        
        
        for (int plant = 0; plant < pE.GetLength(0); plant++)
        {
            
            foreach (EarthPlant earthPlant in t)
            {
                if (earthPlant.GetIndex() == pE[plant, 3])
                {
                                        
                    var a = Instantiate(earthPlant, new Vector3(pE[plant, 0], pE[plant, 1], pE[plant, 2]), new Quaternion(rE[plant, 0], rE[plant, 1], rE[plant, 2], rE[plant, 3]));
                    UnityEngine.Debug.Log(a.name);
                    a.transform.localScale = earthPlant.GetScale();
                    a.gameObject.layer = Constants.intObjLayer;
                }               
            }
        }

        var en = FindObjectsOfType<Enigma>();

        i = 0;
        //UnityEngine.Debug.Log(en.Length + " " + enigmasComplete.Length);
        foreach (Enigma enigma in en)
        {
            //UnityEngine.Debug.Log(i);
            enigma.Complete(enigmasComplete[i,0], enigmasComplete[i,1]);
            i++;
        }
    }

    public void ResetGame()
    {
        character.NewGame();
        EventManager.TriggerEvent("Reset");
    }
}
