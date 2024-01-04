using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField]
    private EnemySpawner[] spawners;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == Constants.protagonistLayer)
        {
            foreach (var spawner in spawners)
            {
                spawner.StartSpawner();
            }
        }
    }
}
