using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Save : MonoBehaviour
{

    public GameObject saveIcon;
    public void SaveState()
    {
        int diams;
        float[,] posEarth;
        float[,] rotEarth;
        float[,] posAir;
        float[,] rotAir;
        int[] fireData;
        bool[] waterData;
        float[] playerPos;
        float[] playerRot;
        bool[,] enigmasComplete;
        int checkpoint;
        

        playerPos = new float[3];
        playerRot = new float[4];

        diams = GameManager.instance.character.GetDiamonds();

        var p = GameManager.instance.character.transform.position;
        playerPos[0] = p.x;
        playerPos[1] = p.y;
        playerPos[2] = p.z;

        var r = GameManager.instance.character.transform.rotation;
        playerRot[0] = r.x;
        playerRot[1] = r.y;
        playerRot[2] = r.z;
        playerRot[3] = r.w;

        int i = 0;

        var x = FindObjectsOfType<EarthPlant>();

        posEarth = new float[x.Length, 4];
        rotEarth = new float[x.Length, 4];

        foreach (EarthPlant e in x)
        {
            posEarth[i, 0] = e.transform.position.x;
            posEarth[i, 1] = e.transform.position.y;
            posEarth[i, 2] = e.transform.position.z;
            posEarth[i, 3] = e.GetIndex();

            rotEarth[i, 0] = e.transform.rotation.x;
            rotEarth[i, 1] = e.transform.rotation.y;
            rotEarth[i, 2] = e.transform.rotation.z;
            rotEarth[i, 3] = e.transform.rotation.w;

            i++;
        }

        var y = FindObjectsOfType<Air>();

        posAir = new float[y.Length, 3];
        rotAir = new float[y.Length, 3];

        i = 0;

        foreach (Air a in y)
        {
            posAir[i, 0] = a.transform.position.x;
            posAir[i, 1] = a.transform.position.y;
            posAir[i, 2] = a.transform.position.z;

            rotAir[i, 0] = a.transform.rotation.x;
            rotAir[i, 1] = a.transform.rotation.y;
            rotAir[i, 2] = a.transform.rotation.z;

            i++;
        }

        var fires = FindObjectsOfType<Fire>(includeInactive:true);
        var z = fires.OrderBy(fire => fire.GetComponent<EnigmaObj>()?.value).Where(fire => fire.gameObject.layer == Constants.intObjLayer && !fire.GetComponent<EarthPlant>()).ToArray();
        
        fireData = new int[z.Length];        

        i = 0;
        foreach (Fire f in z)
        {
            //Debug.Log(i + " " + f.name + " " + f.GetFire());
            fireData[i] = f.GetFire();

            //if (f.GetFire() == 1)
            //    Debug.Log(f.name + " acceso");

            i++;
        }

        var s = FindObjectsOfType<WaterObj>();

        i = 0;

        waterData = new bool[s.Length];

        foreach (WaterObj w in s)
        {
            waterData[i] = w.IsRise();

            i++;
        }

        checkpoint = GameManager.instance.character.GetCheckpointIndex();

        var en = FindObjectsOfType<Enigma>();

        enigmasComplete = new bool[en.Length, 2];
        i = 0;
        foreach (Enigma obj in en)
        {
            var enigma = obj.IsComplete();
            enigmasComplete[i, 0] = enigma[0];
            enigmasComplete[i, 1] = enigma[1];
            i++;
        }

        Data d = new Data(diams, playerPos, playerRot, posEarth, rotEarth, posAir, rotAir, fireData, waterData, checkpoint, enigmasComplete);
        
        DataManager.saveData(d);
        StartCoroutine(SaveText());
    }

    public IEnumerator SaveText()
    {
        saveIcon.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        saveIcon.gameObject.SetActive(false);
    }
}
