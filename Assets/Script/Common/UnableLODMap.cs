using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnableLODMap : MonoBehaviour
{
    public Camera specificCamera; // The specific camera
    //public string treeTag = "Tree"; // The tag for the type of tree

    void Update()
    {
        if (Camera.current == specificCamera)
        {
            //GameObject[] trees = GameObject.FindGameObjectsWithTag(treeTag);
            LODGroup[] trees = FindObjectsOfType<LODGroup>();
            foreach (LODGroup tree in trees)
            {
                
                if (tree != null)
                {
                    tree.enabled = false;
                }
            }
        }
        else
        {
            //GameObject[] trees = GameObject.FindGameObjectsWithTag(treeTag);
            LODGroup[] trees = FindObjectsOfType<LODGroup>();
            foreach (LODGroup tree in trees)
            {
               
                
                if (tree != null)
                {
                    tree.enabled = true;
                }
            }
        }
    }
}

