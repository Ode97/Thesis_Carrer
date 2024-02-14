using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class Data
{
    //0 = fire(open/close), 1 = water(rise), 2 = air(pos, rot), 3 = earthPlant (pos, rot)
    
    public int diamonds;
    public int life;
    public float[] playerPos = new float[3];
    public float[] playerRot = new float[3];
    public float[,] fairiesPos;
    public float[,] earthPos;
    public float[,] earthRot;
    public float[,] airPos;
    public float[,] airRot;
    public int[] fireData;
    public bool[] waterData;
    public bool[] bookData;
    public int[] enemies;
    public float[,] sheepPos;
    public float[,] sheepRot;
    public bool[] checkpoints;
    public int checkpoint;
    public bool[,] enigmasComplete;
    public bool[] spotsOK;
    public bool endEnemyCheck;

    public Data(int d, int l, float[,] fP,float[] pPos, float[] pRot, float[,] pE, float[,] rE, float[,] aP, float[,] aR, int[] fD, bool[] wD, bool[] bD, int[]E, float[,] sP, float[,] sR, bool[] checks, int cP, bool[,] enigmaC, bool[] sOk, bool eC)
    {

        earthPos = pE;
        earthRot = rE;
        playerPos = pPos;
        playerRot = pRot;
        fairiesPos = fP;
        diamonds = d;
        life = l;
        airPos = aP;
        airRot = aR;
        fireData = fD;
        waterData = wD;
        bookData = bD;
        enemies = E;
        sheepPos = sP;
        sheepRot = sR;
        checkpoints = checks;
        checkpoint = cP;
        enigmasComplete = enigmaC;
        spotsOK = sOk;
        endEnemyCheck = eC;

    }
    
    
}
