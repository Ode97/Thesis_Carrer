using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : MagicInteractable
{
    public GameObject tree;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    override
    public void MagicInteraction(MagicElement element)
    {
        if (element.element == Element.Earth)
        {
            var t = Instantiate(tree, transform);
            t.transform.position = element.pos;
            t.transform.localScale = new Vector3(2, 2, 2);
        }
    }
}
