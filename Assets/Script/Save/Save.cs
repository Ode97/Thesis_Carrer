using System;
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
        bool[] bookData;
        float[] playerPos;
        float[] playerRot;
        float[,] fairiesPos;
        int life;
        int[] enemies;
        float[,] sheepPos;
        float[,] sheepRot;
        bool[,] enigmasComplete;
        bool[] checkpoints;
        bool[] spotsOK;
        int checkpoint;

        fairiesPos = new float[3, 3];

        var fairies = FindObjectsOfType<Fairy>();

        int i = 0;
        Array.Sort(fairies, (a, b) => a.gameObject.name.CompareTo(b.gameObject.name));
        foreach (var fai in fairies)
        {
            fairiesPos[i, 0] = fai.GetTarget().x;
            fairiesPos[i, 1] = fai.GetTarget().y;
            fairiesPos[i, 2] = fai.GetTarget().z;
            i++;
        }

        playerPos = new float[3];
        playerRot = new float[4];

        diams = GameManager.instance.character.GetDiamonds();

        life = GameManager.instance.character.health;

        var p = GameManager.instance.character.transform.position;
        playerPos[0] = p.x;
        playerPos[1] = p.y;
        playerPos[2] = p.z;

        var r = GameManager.instance.character.transform.rotation;
        playerRot[0] = r.x;
        playerRot[1] = r.y;
        playerRot[2] = r.z;
        playerRot[3] = r.w;

        i = 0;

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

        Array.Sort(y, (a, b) => a.gameObject.name.CompareTo(b.gameObject.name));
        //Debug.Log("save: " + y.ToString());
        posAir = new float[y.Length, 3];
        rotAir = new float[y.Length, 3];

        i = 0;

        foreach (Air a in y)
        {
            //Debug.Log(a.name);
            posAir[i, 0] = a.transform.position.x;
            posAir[i, 1] = a.transform.position.y;
            posAir[i, 2] = a.transform.position.z;

            rotAir[i, 0] = a.transform.rotation.x;
            rotAir[i, 1] = a.transform.rotation.y;
            rotAir[i, 2] = a.transform.rotation.z;

            i++;
        }

        var fires = FindObjectsOfType<Fire>(includeInactive:true);
        fires = fires.Where(fire => fire.gameObject.layer == Constants.intObjLayer && !fire.GetComponent<EarthPlant>()).ToArray();
        //var z = fires.OrderBy(fire => fire.GetComponent<EnigmaObj>()?.value).Where(fire => fire.gameObject.layer == Constants.intObjLayer && !fire.GetComponent<EarthPlant>()).ToArray();
        Array.Sort(fires, (x, y) => {
            // Ottieni i componenti EnigmaObj
            EnigmaObj ex = x.GetComponent<EnigmaObj>();
            EnigmaObj ey = y.GetComponent<EnigmaObj>();

            if (ex == null || ey == null)
            {
                // Se uno dei componenti non esiste, confronta i nomi dei GameObjects
                return x.name.CompareTo(y.name);
            }

            // Confronta i valori
            int valueComparison = ex.value.CompareTo(ey.value);
            if (valueComparison != 0)
            {
                // Se i valori non sono uguali, ritorna il risultato del confronto
                return valueComparison;
            }
            else
            {
                // Se i valori sono uguali, confronta i nomi
                return x.name.CompareTo(y.name);
            }
        });

        

        var z = fires;
        fireData = new int[z.Length];        

        i = 0;
        foreach (Fire f in z)
        {           
            fireData[i] = f.GetFire();

            //if (f.GetFire() == 1)
            //    Debug.Log(f.name + " acceso");

            i++;
        }

        var s = FindObjectsOfType<WaterObj>();
        Array.Sort(s, (a, b) => a.gameObject.name.CompareTo(b.gameObject.name));
        i = 0;

        waterData = new bool[s.Length];

        foreach (WaterObj w in s)
        {
            waterData[i] = w.IsRise();

            i++;
        }

        var t = FindObjectsOfType<Book>(includeInactive: true);
        Array.Sort(t, (a, b) => a.gameObject.name.CompareTo(b.gameObject.name));

        i = 0;
        bookData = new bool[t.Length];

        foreach (Book b in t)
        {
            
            bookData[i] = b.IsTaken();

            i++;
        }

        var enem = FindObjectsOfType<Enemy>(includeInactive: true);
        Array.Sort(enem, (a, b) => a.gameObject.name.CompareTo(b.gameObject.name));
        i = 0;
        enemies = new int[enem.Length];

        foreach (Enemy e in enem)
        {
            enemies[i] = e.GetHealth();

            i++;
        }

        var sheeps = FindObjectsOfType<Sheep>();

        Array.Sort(sheeps, (a, b) => a.gameObject.name.CompareTo(b.gameObject.name));
        i = 0;
        sheepPos = new float[sheeps.Length, 3];
        sheepRot = new float[sheeps.Length, 4];
        foreach (Sheep sh in sheeps)
        {
            sheepPos[i, 0] = sh.transform.localPosition.x;
            sheepPos[i, 1] = sh.transform.localPosition.y;
            sheepPos[i, 2] = sh.transform.localPosition.z;

            sheepRot[i, 0] = sh.transform.localRotation.x;
            sheepRot[i, 1] = sh.transform.localRotation.y;
            sheepRot[i, 2] = sh.transform.localRotation.z;
            sheepRot[i, 3] = sh.transform.localRotation.w;

            i++;
        }


        var checks = FindObjectsOfType<Checkpoint>();
        Array.Sort(checks, (a, b) => a.gameObject.name.CompareTo(b.gameObject.name));
        i = 0;
        checkpoints = new bool[checks.Length];

        foreach (Checkpoint c in checks)
        {
            checkpoints[i] = c.IsDiscovered();

            i++;
        }

        checkpoint = GameManager.instance.character.GetCheckpointIndex();

        var spots = FindObjectsOfType<Spot>();
        Array.Sort(spots, (a, b) => a.gameObject.name.CompareTo(b.gameObject.name));
        i = 0;
        spotsOK = new bool[spots.Length];
        foreach (Spot sp in spots)
        {        
            spotsOK[i] = sp.isCorrect();
            i++;
        }


        var en = FindObjectsOfType<Enigma>();
        Array.Sort(en, (a, b) => a.gameObject.name.CompareTo(b.gameObject.name));
        enigmasComplete = new bool[en.Length, 2];
        i = 0;
        

        foreach (Enigma obj in en)
        {
            
            var enigma = obj.IsComplete();
            
            enigmasComplete[i, 0] = enigma[0];
            enigmasComplete[i, 1] = enigma[1];
            i++;
        }

        var enemyCheck =  FindObjectOfType<CheckEnemyDeath>().IsFinish();

        Data d = new Data(diams, life, fairiesPos, playerPos, playerRot, posEarth, rotEarth, posAir, rotAir, fireData, waterData, bookData, enemies, sheepPos, sheepRot, checkpoints, checkpoint, enigmasComplete, spotsOK, enemyCheck);
        
        DataManager.saveData(d);
        StartCoroutine(SaveText());
    }

    public IEnumerator SaveText()
    {
        saveIcon.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        saveIcon.gameObject.SetActive(false);
    }
}
