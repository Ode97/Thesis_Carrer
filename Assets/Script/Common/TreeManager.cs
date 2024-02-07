using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    
    //public GameObject treePrefab; // The tree prefab with various LODs
    public Terrain[] terrains; // The terrain

    void Start()
    {
        if (!Application.isEditor)
        {
            TreeInstance[] treeInstances;
            // Get the tree instances from the terrain
            foreach (var terrain in terrains)
            {
                treeInstances = terrain.terrainData.treeInstances;

                // Loop through each tree instance
                foreach (TreeInstance treeInstance in treeInstances)
                {
                    TreePrototype treePrototype = terrain.terrainData.treePrototypes[treeInstance.prototypeIndex];

                    // Ora treePrototype.prototype contiene l'asset dell'albero
                    GameObject treeAsset = treePrototype.prefab;
                    // Calculate the world position of the tree instance
                    Vector3 position = Vector3.Scale(treeInstance.position, terrain.terrainData.size) + terrain.transform.position;

                    // Instantiate the tree prefab at the position of the tree instance
                    GameObject tree = Instantiate(treeAsset, position, Quaternion.identity);

                    //if(treeInstance.prototypeIndex != 0)
                    if(tree.gameObject.name.Contains("PT_Pine_Tree_03_green(Clone)"))
                        tree.transform.localScale = Vector3.one * treeInstance.widthScale * 3;
                    else
                        tree.transform.localScale = Vector3.one * treeInstance.widthScale;
                }

                // Remove the tree instances from the terrain
                terrain.terrainData.treeInstances = new TreeInstance[0];

            }
        }
    }
}

