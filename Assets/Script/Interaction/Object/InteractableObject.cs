using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    //private Outline outline;
    private bool onView = false;
    private float timer = 0;
    private bool start = false;
    protected Color color;
    //private bool isTerrain = false;
    protected Element element;

    // Start is called before the first frame update
    void Awake()
    {
        /*if (gameObject.layer != LayerMask.NameToLayer("UI"))
        {
            if (!GetComponent<Outline>())
                outline = gameObject.AddComponent<Outline>();
            else
                outline = gameObject.GetComponent<Outline>();


            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = Color.yellow;
            outline.OutlineWidth = 5f;
            outline.enabled = false;
        }*/
        EventManager.StartListening("WrongEnigma" + gameObject.name, EnigmaFail);
        EventManager.StartListening("Reset", Reset);
        //Earth earth;
        //isEarth = TryGetComponent<Earth>(out earth);
        //isTerrain = gameObject.layer;
    }

    // Update is called once per frame
    protected virtual void Update()
    {       
        if (start && GameManager.instance.IsInteraction() && gameObject.layer != Constants.terrainLayer && (GameManager.instance.character.GetActualElement() == element || (GameManager.instance.airEffect && gameObject == GameManager.instance.character.GetMagicElement().GetObject().gameObject)))
        {
            
            GameManager.instance.selectionSlider.SetActive(true);
            timer += Time.deltaTime;
            GameManager.instance.SetSliderTime(timer);

            if (IsOnView())
            {
                
                if (!GameManager.instance.stopLogic)
                    GameManager.instance.fixing = true;
                start = false;
                StartCoroutine(Wait());
            }
        }
    }

    public IEnumerator ResetTimer()
    {
        yield return new WaitForFixedUpdate();
        timer = 0;
    }

    private IEnumerator Wait()
    {
        yield return new WaitForFixedUpdate();
        timer = 0;
    }

    public abstract bool Interaction();

    public abstract bool WaterInteraction();

    public abstract bool FireInteraction();

    public abstract bool AirInteraction();

    public abstract bool EarthInteraction(GameObject obj, Vector3 pos);

    public abstract void Reset();

    protected virtual void EnigmaFail()
    {
        Debug.Log("aa");
    }

    public static bool click = false;

    private void OnMouseEnter()
    {
        
        if (!MenuManager.instance.isMenuOpen())
        {
            if (!click)
            {
                start = true;
            }
            if (gameObject.layer != Constants.terrainLayer && GameManager.instance.IsInteraction())
            {
                //GameManager.instance.selectionSlider.SetActive(true);
                    
                timer = 0;
                GameManager.instance.SetSliderTime(timer);
            }
            if (GameManager.instance.IsInteraction() && !onView)
            {
                    
                onView = true;
                GameManager.instance.SetObject(this);
            }

            if (!GameManager.instance.airEffect)
            {
                GameManager.instance.outlineEffect.SetActive(true);
                ParticleSystem.MainModule main = GameManager.instance.outlineEffect.GetComponent<ParticleSystem>().main;
                main.startColor = color;
                //outline.SetActive(true);
            }
        }      
    }

    private void OnMouseOver()
    {
        if (!MenuManager.instance.isMenuOpen() && GameManager.instance.IsInteraction())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, Mathf.Infinity);
            GameManager.instance.outlineEffect.transform.position = hit.point + new Vector3(0, 2, 0);
            /*if (!isTerrain)
            {
                //GameManager.instance.selectionSlider.SetActive(true);                
            }*/
        }else
            GameManager.instance.outlineEffect.SetActive(false);
    }

    void OnMouseExit()
    {
        if (!MenuManager.instance.isMenuOpen())
        {
            GameManager.instance.selectionSlider.SetActive(false);
            
            start = false;
            GameManager.instance.fixing = false;
            timer = 0;
            GameManager.instance.SetSliderTime(timer);
            GameManager.instance.outlineEffect.SetActive(false);
            if (GameManager.instance.IsInteraction())
                StartCoroutine(EndSelection());
        }

    }

    private IEnumerator EndSelection()
    {
        onView = false;
        yield return new WaitForSeconds(0.1f);
        if (!onView)
        {
            /*if (outline)
            {
                outline.SetActive(false);
            }*/
            //if(outline)
            //    outline.enabled = false;
            GameManager.instance.EndSelection();
        }
    }

    public bool IsOnView()
    {
        
        return timer > GameManager.instance.fixingTime;
    }

    /*public void StartSelection()
    {
        outline.enabled = true;
    }

    public void EndSelection()
    {
        outline.enabled = false;
    }*/
}
