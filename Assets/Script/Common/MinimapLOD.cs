using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MinimapLOD : MonoBehaviour
{
    public LODGroup tree1;
    public LODGroup tree2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLodMap() 
    {        
  
        tree1.ForceLOD(1);
        tree2.ForceLOD(1);
        
        
    }

    public void SetNormalLod()
    {
        tree1.ForceLOD(-1);
        tree2.ForceLOD(-1);
     
    }
}
