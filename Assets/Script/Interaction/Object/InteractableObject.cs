using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    private Outline outline;
    private bool on = false;
    // Start is called before the first frame update
    void Awake()
    {
        outline = gameObject.AddComponent<Outline>();

        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = Color.yellow;
        outline.OutlineWidth = 5f;
        outline.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract bool Interaction();

    public abstract bool WaterInteraction();

    public abstract bool FireInteraction();

    public abstract bool AirInteraction();

    public abstract bool EarthInteraction(GameObject obj, Vector3 pos);


    void OnMouseEnter()
    {
        if (GameManager.instance.IsInteraction() && !on)
        {
            //Debug.Log("add: " + name);
            on = true;
            GameManager.instance.SetObject(this);
        }

        outline.enabled = true;
    }

    void OnMouseExit()
    {
        if (GameManager.instance.IsInteraction())
            StartCoroutine(EndSelection());
        else
            outline.enabled = false;
    }

    private IEnumerator EndSelection()
    {
        on = false;
        yield return new WaitForSeconds(0.3f);
        if (!on)
        {
            outline.enabled = false;
            GameManager.instance.EndSelection();
        }
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
