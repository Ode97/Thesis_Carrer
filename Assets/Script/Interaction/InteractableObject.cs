using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void Interaction();

    public abstract void WaterInteraction();

    public abstract void FireInteraction();

    public abstract void AirInteraction(Vector3 force);

    public abstract void EarthInteraction(GameObject obj, Vector3 pos);
}
