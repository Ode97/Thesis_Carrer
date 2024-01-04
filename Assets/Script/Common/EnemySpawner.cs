using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    private Vector3 position;
    [SerializeField]
    private Quaternion rotation;
    private int i = 0;
    [SerializeField]
    public int target;
    [SerializeField]
    public int spawnTime = 10;

    private bool spawn = false;
    private bool end = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!end && !spawn)
        {
            if (i < target)
                StartCoroutine(Spawn());
            else
                end = true;
        }
    }

    private IEnumerator Spawn()
    {
        spawn = true;
        var e = Instantiate(enemy, transform);
        e.GetComponent<Enemy>().SetEnigma();
        //e.transform.localPosition = position;
        //e.transform.localRotation = rotation;

        i++;
        yield return new WaitForSeconds(spawnTime);
        spawn = false;       
        
    }

    public void StartSpawner()
    {
        end = false;
    }

    
}
