using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int health;

    private Inventory inventory;

    private GameObject aurea;

    private MagicElement actualElement;

    private Movment movment;
    private Animator animator;
    private bool activeElement = false;
    private bool stopAurea = false;
    private int nAureas = 0;

    // Start is called before the first frame update
    void Start()
    {
        movment = GetComponent<Movment>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!stopAurea && activeElement)
        {
            nAureas++;
            StartCoroutine(StopAurea());
        }
    }

    public IEnumerator StopAurea()
    {
        stopAurea = true;
        yield return new WaitForSeconds(10);
        nAureas--;
        if (nAureas == 0)
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

        
        activeElement = true;

        stopAurea = false;
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

    public void Interaction(RaycastHit hit)
    {
        InteractableObject obj = hit.collider.gameObject.GetComponent<InteractableObject>();
        if (activeElement)
        {
            actualElement.SetObject(obj);
            actualElement.SetPosition(hit.point);
            actualElement.ApplyEffect();
        }
        else
            obj.Interaction();
    }

}
