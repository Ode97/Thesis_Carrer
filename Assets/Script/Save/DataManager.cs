using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;


public class DataManager : MonoBehaviour {
    public static void saveData(Data g){

        string destination = Application.persistentDataPath + "/" + "_game.dat";

        //Debug.Log(destination);

        FileStream file;

        if(File.Exists(destination)) 
            file = File.OpenWrite(destination);
        else 
            file = File.Create(destination);


        Data data = g;
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
        //GameManager.GM().audioManager.Confirm();
    }
    
    public static Data loadData(){
        string destination = Application.persistentDataPath + "/" + "_game.dat";
        FileStream file;

        if(File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            //Debug.LogError("Save File not found");
            return null;
        }

        BinaryFormatter bf = new BinaryFormatter();
        Data data = (Data) bf.Deserialize(file);
        
        file.Close();

        GameManager.instance.SetLoad(data.diamonds, data.life, data.fairiesPos, data.playerPos, data.playerRot, data.earthPos, data.earthRot, data.airPos, data.airRot, data.fireData, data.waterData, data.bookData, data.enemies, data.sheepPos, data.sheepRot, data.checkpoints, data.checkpoint, data.enigmasComplete, data.spotsOK, data.endEnemyCheck);
        //GameManager.GM().data = data;
        return data;        
    }


    public static void DeleteFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";

        if (File.Exists(destination)) File.Delete(destination);
        else
        {
            //Debug.LogError("File not found");
            return;
        }
    }
}
