using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class Savemanager
{
    public static string Directory = "/SaveData/";
    public static string FileName = "MyData.txt";

    public static void Save(SaveObject so)
    {
        string dir = Application.persistentDataPath + Directory;

        if (System.IO.Directory.Exists(dir))
            System.IO.Directory.CreateDirectory(dir);

        string json = JsonUtility.ToJson(so);
        File.WriteAllText(dir + FileName, json);
    }

    public static SaveObject Load()
    {
        string fullpath = Application.persistentDataPath + Directory + FileName;
        SaveObject so = new SaveObject();

        if (File.Exists(fullpath))
        {
            string json = File.ReadAllText(fullpath);
            so = JsonUtility.FromJson<SaveObject>(json);
        }
        return so;
    }
}
