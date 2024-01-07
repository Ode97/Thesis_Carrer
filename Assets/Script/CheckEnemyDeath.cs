using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckEnemyDeath : MonoBehaviour
{
    [SerializeField]
    private Enemy[] enemies;
    private int init = 0;
    private int i = 0;
    bool finish = false;
    // Start is called before the first frame update
    void Start()
    {
        init = enemies.Length;
        i = enemies.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (!finish)
        {
            foreach (Enemy enemy in enemies)
            {
                if (enemy.IsDestroyed())
                    i--;
            }
            if (i == 0)
            {
                GetComponent<DialogueTrigger>().TriggerDialogue();
                finish = true;
            }
            else
                i = init;
        }
    }
}
