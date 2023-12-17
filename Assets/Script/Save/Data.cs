using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class Data
{
    //0 = fire(open/close), 1 = water(rise), 2 = air(pos, rot), 3 = earthPlant (pos, rot)
    
    public int diamonds;
    public float[] playerPos = new float[3];
    public float[] playerRot = new float[3];
    public float[,,] earthPos;
    public float[,,] earthRot;
    public float[,] airPos;
    public float[,] airRot;
    public int[] fireData;
    public bool[] waterData;
    public int checkpoint;

    public Data(int d, float[] pPos, float[] pRot, float[,,] pE, float[,,] rE, float[,] aP, float[,] aR, int[] fD, bool[] wD, int cP)
    {

        earthPos = pE;
        earthRot = rE;
        playerPos = pPos;
        playerRot = pRot;
        diamonds = d;
        airPos = aP;
        airRot = aR;
        fireData = fD;
        waterData = wD;
        checkpoint = cP;

    }
    
    
}
