using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class CreateCSV
{
    //private StringBuilder csv;
    string csvFilePath = Path.Combine(Application.dataPath, "output2.csv");
    private List<string> rows = new List<string>();

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
        rows.Add(row);
        
    }

    public void EndCSV()
    {
        using (StreamWriter writer = new StreamWriter(csvFilePath, true))
        {
            foreach (string row in rows)
            {
                writer.WriteLine(row);
            }
        }
    }
}
