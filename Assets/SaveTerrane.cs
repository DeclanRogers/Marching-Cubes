using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveTerrane : MonoBehaviour
{
    public MeshGeneration meshGen;



    public void LoadFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        StreamReader file;

        string line;

        if (File.Exists(destination)) file = new StreamReader(destination);
        else
        {
            Debug.LogError("File not found");
            return;
        }
        int c = 0;
        while ((line = file.ReadLine()) != null)
        {
            //print(line);
            c++;
        }
        print(c);
    }


}
