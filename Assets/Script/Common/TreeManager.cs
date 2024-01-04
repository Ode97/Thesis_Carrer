using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    
    public GameObject treePrefab; // The tree prefab with various LODs
    public Terrain terrain; // The terrain

    void Start()
    {
        // Get the tree instances from the terrain
        TreeInstance[] treeInstances = terrain.terrainData.treeInstances;

        // Loop through each tree instance
        foreach (TreeInstance treeInstance in treeInstances)
        {
            // Calculate the world position of the tree instance
            Vector3 position = Vector3.Scale(treeInstance.position, terrain.terrainData.size) + terrain.transform.position;

            // Instantiate the tree prefab at the position of the tree instance
            GameObject tree = Instantiate(treePrefab, position, Quaternion.identity);

            // Set the scale of the tree
            tree.transform.localScale = Vector3.one * treeInstance.widthScale;
        }

        // Remove the tree instances from the terrain
        terrain.terrainData.treeInstances = new TreeInstance[0];

        GetComponent<UnableLODMap>().enabled = true;
    }
}

