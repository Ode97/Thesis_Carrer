using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    //private Outline outline;
    private bool onView = false;
    private float timer = 0;
    private bool start = false;
    protected GameObject outlineEffect;
    
    // Start is called before the first frame update
    void Awake()
    {
        outlineEffect = Instantiate(Resources.Load<GameObject>("CFX_Magical_Source Variant"));
        outlineEffect.SetActive(false);
        

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
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (start && GameManager.instance.IsInteraction())
        {
            timer += Time.deltaTime;

            if (timer > 2 && !GetComponent<Earth>())
            {
                //Debug.Log(timer);
                Debug.Log("select: " + name);
                if(!GameManager.instance.stopLogic)
                    GameManager.instance.fixing = true;
                start = false;
                StartCoroutine(Wait());
            }
        }
        
            
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


    void OnMouseEnter()
    {
        if (!Settings.instance.isMenuOpen())
        {
            start = true;
            timer = 0;
            if (GameManager.instance.IsInteraction() && !onView)
            {

                onView = true;
                GameManager.instance.SetObject(this);
            }

            if (outlineEffect)
            {
                outlineEffect.SetActive(true);
            }

            //if (outline)
            //    outline.enabled = true;
        }
    }

    private void OnMouseOver()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, Mathf.Infinity);
        outlineEffect.transform.position = hit.point + new Vector3(0, 2, 0);
    }

    void OnMouseExit()
    {
        start = false;
        GameManager.instance.fixing = false;
        timer = 0;
        if (GameManager.instance.IsInteraction())
            StartCoroutine(EndSelection());
        else if (outlineEffect)
        {
            outlineEffect.SetActive(false);
        }
        //else if(outline)
        //    outline.enabled = false;
    }

    private IEnumerator EndSelection()
    {
        onView = false;
        yield return new WaitForSeconds(0.2f);
        if (!onView)
        {
            if (outlineEffect)
            {
                outlineEffect.SetActive(false);
            }
            //if(outline)
            //    outline.enabled = false;
            GameManager.instance.EndSelection();
        }
    }

    public bool IsOnView()
    {
        return timer > 2;
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
