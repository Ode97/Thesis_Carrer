using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirElement : MagicElement
{
    public float power;
    private bool clicked = true;
    public Vector3 destination;

    override
    public void ApplyEffect()
    {

        clicked = false;
        
        
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0) && !clicked)
        {
            clicked = true;

            // Cast a ray from the mouse position into the scene
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, Mathf.Infinity);

            var direction = (hit.point - interactableObject.transform.position).normalized;

            Vector3 force = direction * power;

            interactableObject.AirInteraction(force);
        }
    }
}
