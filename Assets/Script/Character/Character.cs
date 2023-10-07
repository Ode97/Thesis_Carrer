using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int health;

    private Inventory inventory;

    private GameObject aurea;

    private MagicElement actualElement;
    private InteractableObject obj;

    private Movment movment;
    private Animator animator;
    private bool activeElement = false;
    

    // Start is called before the first frame update
    void Start()
    {
        movment = GetComponent<Movment>();
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    

    public void StopImmediateAurea()
    {
        
        if (aurea != null)
        {
            Destroy(aurea.gameObject);
            activeElement = false;
        }
    }

    public void MoveToDestination(Vector3 point)
    {
        movment.Move(point);
    }

    public void SetActualElement(MagicElement element)
    {
        if(aurea != null)
            Destroy(aurea.gameObject);

        if (actualElement != null && element == actualElement)
        {
            actualElement = null;
            activeElement = false;
            return;
        }
        activeElement = true;

        aurea = Instantiate(element.aurea, transform);
        actualElement = element;
    }

    public bool isActiveElement()
    {
        return activeElement;
    }

    public MagicElement getActualElement()
    {
        return actualElement;
    }

    public void SetObject(InteractableObject obj)
    {
        this.obj = obj;
        //Debug.Log(obj.gameObject.name);
    }

    public InteractableObject GetObject()
    {
        return obj;
    }

    public void Interaction(RaycastHit hit)
    {
        if (activeElement)
        {           
            actualElement.SetObject(obj);
            actualElement.SetPosition(hit.point);
            actualElement.ApplyEffect();
        }
        else
        {
            obj.Interaction();
        }
    }

    public void MoveCharacter(Vector3 pos, Vector3 move)
    {
        if(onMoveableSurface)
            transform.position = Vector3.MoveTowards(transform.position, pos, move.magnitude);
    }

    private bool onMoveableSurface = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.GetComponent<Air>())
        {
            onMoveableSurface = true;
        }
        else
            onMoveableSurface = false;
    }
}
