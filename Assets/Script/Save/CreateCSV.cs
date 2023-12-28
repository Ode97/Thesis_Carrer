using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class CreateCSV
{
    //private StringBuilder csv;
    string csvFilePath = Path.Combine(Application.dataPath, "output.csv");


    public CreateCSV()
    {
        

        if (!File.Exists(csvFilePath) || File.ReadAllText(csvFilePath).Length == 0) 
        {
            Debug.Log(Application.dataPath + "/output.csv");
            AddData("timestamp;" + "cameraMode;" + "object;" + "element;" + "isMoving;" + "isAttacking;" + "x;" + "y;");
        }
    }

    public void AddData(string row)
    {

        using (StreamWriter writer = new StreamWriter(csvFilePath, true))
        {
            writer.WriteLine(row);
        }
    }

    

}
