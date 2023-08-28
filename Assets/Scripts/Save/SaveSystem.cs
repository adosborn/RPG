using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer (Player player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath,"player.fun");
        //string path = Application.persistentDataPath + "/player.fun";
        FileStream stream = new FileStream(path, FileMode.Create);
        //Debug.Log("Create the stream");

        SaveData data = new SaveData(player);

        formatter.Serialize(stream, data);
        stream.Close();
        //Debug.Log("Closed the stream");
    }

    public static SaveData LoadPlayer ()
    {
        string path = Path.Combine(Application.persistentDataPath, "player.fun");
        //string path = Application.persistentDataPath + "/player.fun";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Open)) {
                //Debug.Log("Opened the stream");
                SaveData data = formatter.Deserialize(stream) as SaveData;
                stream.Close();
                //Debug.Log("Closed the stream");
                return data;
            } 
        }
        else
        {
            Debug.LogError("Save File Not Found in " + path);
            return null;
        }
    }

}
