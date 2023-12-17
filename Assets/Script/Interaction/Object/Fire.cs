using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Fire : InteractableObject
{
    public GameObject fireParticle;
    public bool distructible = false;
    private int fire = 0;
    private GameObject fireEffect;
    private GameObject actualFire;
    // Start is called before the first frame update
    void Start()
    {
        fireEffect = Resources.Load<GameObject>("WildFire Variant");
        
        if(!distructible)
            fireParticle = transform.GetChild(0).gameObject;

        color = Color.red;
        

    }

    /*override
    protected void Update()
    {
        base.Update();
        if(actualFire) 
        {
            //actualFire.transform.position = Vector3.Lerp(actualFire.transform.position, Vector3.right, 5 * Time.deltaTime);
        }
    }*/


    override
    public bool FireInteraction()
    {
        if (distructible)
            StartCoroutine(Fires());
        else
        {
            fireParticle.SetActive(true);
            
        }
        fire = 1;
        GetComponent<EnigmaObj>()?.Interaction(Element.Fire);
        return true;
    }

    private IEnumerator Fires()
    {
        actualFire = Instantiate(fireEffect, transform);
        actualFire.transform.position = transform.position + new Vector3(0, 2, 0);
        actualFire.transform.rotation = Quaternion.identity;
        
        actualFire.transform.localScale = new Vector3(5, 5, 5);
        yield return new WaitForSeconds(4);
        gameObject.SetActive(false);
        Destroy(actualFire);
    }

    public void LoadDestroy()
    {
        gameObject.SetActive(false);
    }

    override
    protected void EnigmaFail()
    {
        WaterInteraction();
    }

    override
    public bool WaterInteraction()
    {
        fire = 0;
        fireParticle.SetActive(false);
        return true;
        
    }

    public override bool Interaction()
    {
        return false;
    }

    public override bool AirInteraction()
    {
        return false;
    }

    public override bool EarthInteraction(GameObject obj, Vector3 pos)
    {
        return false;
    }

    public int GetFire()
    {
        return fire;
    }
}
