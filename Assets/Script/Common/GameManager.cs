using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Diagnostics;
using System.Linq;
using System;
using UnityEngine.UI;

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
    public bool airEffect = false;
    [SerializeField] public GameObject selectionSlider;


    // Start is called before the first frame update
    public static GameManager instance = null;


    public CreateCSV csvBuilder = new CreateCSV();

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
        selectionSlider.GetComponent<Slider>().maxValue = fixingTime;
    }

    public void SetSliderTime(float t)
    {
        selectionSlider.GetComponent<Slider>().value = t;
    }


        // Start is called before the first frame update
    void Start()
    {
        
        save = GetComponent<Save>();
    }

    void FixedUpdate()
    {
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        //UnityEngine.Debug.Log(timestamp + ";" + actualMode.ToString() + ";" + character.GetObject() + ";" + character.getActualElement() + ";" + character.IsMoving() + ";" + character.IsAttacking() + ";" + Input.mousePosition.x + ";" + Input.mousePosition.y);
        csvBuilder.AddData(timestamp + ";" + actualMode.ToString() + ";" + character.GetObject().name + ";" + character.GetActualElement() + ";" + character.IsMoving() + ";" + character.IsAttacking() + ";" + Input.mousePosition.x + ";" + Input.mousePosition.y);
        selectionSlider.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y + 25, Input.mousePosition.z);
    }


    // Update is called once per frame
    void Update()
    {


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, Mathf.Infinity, ~ignoreLayer);
        
        if (!character.GetObject().gameObject.activeSelf)
        {
            EndSelection();
        }

        //UnityEngine.Debug.Log(stopLogic + " " + MenuManager.instance.isMenuOpen());
        if (!stopLogic)
        {

            
            //if (!stopLogic && !MenuManager.instance.isMenuOpen()) {
            if (!MenuManager.instance.isMenuOpen())
            {
                if (character.IsActiveElement())
                {
                    
                    //Physics.Raycast(ray, out hit, Mathf.Infinity, ~ignoreLayer);
                    //selectionSlider.transform.position = hit.point;

                    if (hit.collider && hit.collider.gameObject.layer == Constants.enemyLayer)
                    {
                        if (hit.collider.gameObject.TryGetComponent<Golem>(out Golem golem))
                        {
                            if (golem.IsStun())
                                character.SetEnemy(hit.collider.gameObject);
                        }
                        else
                            character.SetEnemy(hit.collider.gameObject);
                    }
                }

                if (Input.GetMouseButtonDown(0) || fixing)
                {
                    fixing = false;

                    //sopra UI non faccio nulla
                    if (EventSystem.current.IsPointerOverGameObject())
                    {

                        return;
                    }

                    //Physics.Raycast(ray, out hit, Mathf.Infinity, ~ignoreLayer);
                    //selectionSlider.transform.position = hit.point;

                    //UnityEngine.Debug.Log(hit.collider.gameObject.layer);
                    // Cast a ray from the mouse position into the scene
                    
                    if (hit.collider != null)
                    {

                        if (interaction)
                        {
                            var intObj = hit.collider.GetComponent<InteractableObject>();
                            
                            
                            //cambiare con layer e vedere se si rompe tutto
                            if (intObj)
                            {
                                //UnityEngine.Debug.Log(intObj.name);
                                //UnityEngine.Debug.Log(character.GetObject());
                                StartCoroutine(intObj.ResetTimer());
                                character.Interaction(hit);
                                
                            }
                            return;
                        }

                        var layer = hit.collider.gameObject.layer;

                        if (actualMode == CameraMode.Strategica)
                        {

                           
                            if (layer == Constants.terrainLayer || layer == Constants.woodStoneLayer)
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
            //UnityEngine.Debug.Log("actual: " + objs[objs.Count - 1].name);
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

    public void SetLoad(int diamond, int life, float[,] fairiesPos, float[] pPos, float[] pRot, float[,] pE, float[,] rE, float[,] aP, float[,] aR, int[] fD, bool[] wD, bool[] bD, int[] enemyData, float[,] sheepPos, float[,] sheepRot, bool[] checkpoints, int cP, bool[,] enigmasComplete, bool[] spotsOK, bool enemyCheck)
    {
        character.SetDiamonds(diamond);
        character.SetHealth(life);

        character.transform.rotation = new Quaternion(pRot[0], pRot[1], pRot[2], pRot[3]);
        character.transform.position = new Vector3(pPos[0], pPos[1], pPos[2]);

        var fairies = FindObjectsOfType<Fairy>();
        Array.Sort(fairies, (a, b) => a.gameObject.name.CompareTo(b.gameObject.name));
        int i = 0;

        foreach (var fai in fairies)
        {
            fai.transform.position = new Vector3(fairiesPos[i, 0], fairiesPos[i, 1], fairiesPos[i, 2]);
            i++;
        }

        var air = FindObjectsOfType<Air>();
        Array.Sort(air, (a, b) => a.gameObject.name.CompareTo(b.gameObject.name));
        i = 0;
        foreach (Air a in air)
        {
            //var w = Instantiate(a.gameObject, new Vector3(aP[i, 0], aP[i, 1], aP[i, 2]), new Quaternion(aR[i, 0], aR[i, 1], aR[i, 2], 1), a.transform.parent);

            a.gameObject.transform.position = new Vector3(aP[i, 0], aP[i, 1], aP[i, 2]);
            a.gameObject.transform.rotation = new Quaternion(aR[i, 0], aR[i, 1], aR[i, 2], 1);

            //StartCoroutine(Wait(a, w));
            i++;
        }

        var fires = FindObjectsOfType<Fire>();
        //fires = fires.OrderBy(fire => fire.GetComponent<EnigmaObj>()?.value).ToArray();
        fires = fires.Where(fire => fire.gameObject.layer == Constants.intObjLayer && !fire.GetComponent<EarthPlant>()).ToArray();
        Array.Sort(fires, (x, y) => {
            // Ottieni i componenti EnigmaObj


            EnigmaObj ex = x.GetComponent<EnigmaObj>();
            EnigmaObj ey = y.GetComponent<EnigmaObj>();

            if (ex == null || ey == null)
            {
                // Se uno dei componenti non esiste, confronta i nomi dei GameObjects
                return x.name.CompareTo(y.name);
            }

            // Confronta i valori
            int valueComparison = ex.value.CompareTo(ey.value);
            if (valueComparison != 0)
            {
                // Se i valori non sono uguali, ritorna il risultato del confronto
                return valueComparison;
            }
            else
            {
                // Se i valori sono uguali, confronta i nomi
                return x.name.CompareTo(y.name);
            }
        });

        i = 0;
        foreach (Fire f in fires)
        {
            
            if (fD[i] == 1)
            {
                
                if (f.distructible)
                {
                    
                    f.LoadDestroy();
                }
                else
                {
                    
                    f.FireInteraction();
                }
            }
            i++;
        }



        if (MenuManager.instance.IsFirstStart())
        {
            var waters = FindObjectsOfType<WaterObj>();
            Array.Sort(waters, (a, b) => a.gameObject.name.CompareTo(b.gameObject.name));
            i = 0;
            foreach (WaterObj w in waters)
            {
                if (wD[i])
                {
                    w.WaterInteraction();
                }
                i++;
            }
        }
        
        var checks = FindObjectsOfType<Checkpoint>();
        Array.Sort(checks, (a, b) => a.gameObject.name.CompareTo(b.gameObject.name));
        i = 0;
        foreach (Checkpoint c in checks)
        {
            c.SetDiscovered(checkpoints[i]);
            if(c.GetIndex() == cP)
            {
                character.SetCheckpoint(c);
            }
            i++;
        }

        var ep = FindObjectsOfType<EarthPlant>(includeInactive:true);
        i = 0;
        for (int plant = 0; plant < pE.GetLength(0); plant++)
        {
            
            foreach (EarthPlant earthPlant in ep)
            {
                if (earthPlant.GetIndex() == pE[plant, 3])
                {

                    var a = Instantiate(earthPlant, new Vector3(pE[plant, 0], pE[plant, 1], pE[plant, 2]), new Quaternion(rE[plant, 0], rE[plant, 1], rE[plant, 2], rE[plant, 3]));
                    //UnityEngine.Debug.Log(a.name);
                    a.transform.localScale = earthPlant.GetScale();
                    a.gameObject.layer = Constants.plantLayer;
                }               
            }
        }

        var books = FindObjectsOfType<Book>(includeInactive: true);
        Array.Sort(books, (a, b) => a.gameObject.name.CompareTo(b.gameObject.name));
        i = 0;
        foreach (Book b in books)
        {
            
            if (bD[i])
            {
                books[i].Taken();
            }
            i++;
        }

        var enemyCheker = FindObjectOfType<CheckEnemyDeath>();

        enemyCheker.SetFinish(enemyCheck);

        var enemies = FindObjectsOfType<Enemy>(includeInactive: true);
        Array.Sort(enemies, (a, b) => a.gameObject.name.CompareTo(b.gameObject.name));
        i = 0;
        foreach (Enemy e in enemies)
        {
            e.SetHealth(enemyData[i]);
            i++;
        }

        var sheeps = FindObjectsOfType<Sheep>();

        Array.Sort(sheeps, (a, b) => a.gameObject.name.CompareTo(b.gameObject.name));
        i = 0;
        //sheepPos = new float[sheeps.Length, 3];
        //sheepRot = new float[sheeps.Length, 4];
        foreach (Sheep sh in sheeps)
        {
            sh.transform.localPosition = new Vector3(sheepPos[i, 0], sheepPos[i, 1], sheepPos[i, 2]);
            sh.transform.localRotation = new Quaternion(sheepRot[i, 0], sheepRot[i, 1], sheepRot[i, 2], sheepRot[i, 3]);

            i++;
        }

        var spots = FindObjectsOfType<Spot>();
        Array.Sort(spots, (a, b) => a.gameObject.name.CompareTo(b.gameObject.name));
        i = 0;
        foreach (Spot spot in spots)
        {
            spot.SetOk(spotsOK[i]);
            i++;
        }

        var tutorialEnd = FindObjectOfType<CheckEnemyDeath>();


        var en = FindObjectsOfType<Enigma>();
        Array.Sort(en, (a, b) => a.gameObject.name.CompareTo(b.gameObject.name));
        i = 0;
        //UnityEngine.Debug.Log(en.Length + " " + enigmasComplete.Length);
        foreach (Enigma enigma in en)
        {
            //UnityEngine.Debug.Log(i);
            enigma.Complete(enigmasComplete[i,0], enigmasComplete[i,1]);
            i++;
        }

        

        StartCoroutine(WaitGameStart());
    }

    public IEnumerator Wait(Air a, GameObject w)
    {
        yield return new WaitForEndOfFrame();
        w.GetComponent<Air>().SetInitPos(a.GetInitPos(), a.GetInitRot());
        Destroy(a.gameObject);
    }

    public IEnumerator WaitGameStart()
    {
        yield return new WaitForEndOfFrame();
        FindObjectOfType<CheckEnemyDeath>().IsFinish();
        yield return new WaitForSeconds(0.5f);
        character.GameStart();
    }

    public void ResetGame()
    {
        character.NewGame();
        EventManager.TriggerEvent("Reset");
    }
}
