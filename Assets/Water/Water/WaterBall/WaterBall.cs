using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class WaterBall : MonoBehaviour
{
    [SerializeField] ParticleSystem _WaterBallParticleSystem;
    [SerializeField] AnimationCurve _SpeedCurve;
    [SerializeField] float _Speed;
    [SerializeField] ParticleSystem _SplashPrefab;
    [SerializeField] ParticleSystem _SpillPrefab;
    [SerializeField] WaterBallControll _Controll;

    private void Update()
    {
        if (_Controll == null)
            Destroy(gameObject);
    }
    public void Throw(Vector3 target)
    {
        StopAllCoroutines();
        StartCoroutine(Coroutine_Throw(target));
    }

    public void WaterControl(WaterBallControll water)
    {
        _Controll = water;
    }

    IEnumerator Coroutine_Throw(Vector3 t) 
    {
        float lerp = 0;
        startPos = transform.position;
        target = t;
        var dir = target - transform.position;
        
        //target += dir * 0.01f;
        while (lerp < 1)
        {
            //transform.position = Vector3.Lerp(startPos, target, _SpeedCurve.Evaluate(lerp));
            

            transform.position += (Time.deltaTime * _Speed) * dir;

            float magnitude = (transform.position - target).magnitude;
            if (magnitude < 0.1f)
            {
                //break;
            }
            lerp += Time.deltaTime;
            
            yield return null;
        }


        /*_WaterBallParticleSystem.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
        ParticleSystem splas = Instantiate(_SplashPrefab, target, Quaternion.identity);
        Vector3 forward = target - startPos;
        forward.y = 0;
        splas.transform.forward = forward;
        if (Vector3.Angle(startPos - target, Vector3.up) > 30)
        {
            ParticleSystem spill = Instantiate(_SpillPrefab, target, Quaternion.identity);
            spill.transform.forward = forward;
        }
        Destroy(gameObject, 0.5f);*/
    }

    private Vector3 startPos;
    private Vector3 target;

    public void DestroyBall()
    {
        _WaterBallParticleSystem.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
        ParticleSystem splas = Instantiate(_SplashPrefab, target, Quaternion.identity);
        Vector3 forward = target - startPos;
        forward.y = 0;
        splas.transform.forward = forward;
        if (Vector3.Angle(startPos - target, Vector3.up) > 30)
        {
            ParticleSystem spill = Instantiate(_SpillPrefab, target, Quaternion.identity);
            spill.transform.forward = forward;
        }
        Destroy(gameObject, 0.5f);
    }

}
