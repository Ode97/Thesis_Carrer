using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static AudioManager instance = null;
    public AudioSource forest;
    public AudioSource boss;
    public AudioSource bossActivation;
    public AudioSource bossStun;


    void Awake()
    {

        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        forest.Play();
    }

    public void PlayBossMusic()
    {
        forest.Stop();
        boss.Play();

    }

    public void PlayForestMusic()
    {
        boss.Stop();
        forest.Play();
    }

    public void PlayBossAcivation()
    {
        bossActivation.Play();
    }

    public void PlayBossStunned()
    {
        bossStun.Play();
    }
}
