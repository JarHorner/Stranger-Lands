using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    #region Variables

    private static bool loadedGame;
    private static PlayerData currentPlayerData;
    private static string currentFileName;

    #endregion

    #region Methods

    // saves player by taking the player data and streaming it to a file
    public static void SavePlayer(PlayerController player, PlayerCamera camera) // InventoryManager inventoryManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + currentFileName;
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player, camera); // , inventoryManager);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    // loads the selected save file
    public static PlayerData LoadPlayer(string manualFileName = null)
    {
        string path;
        if (manualFileName != null)
            path = Application.persistentDataPath + manualFileName;
        else
            path = Application.persistentDataPath + currentFileName;

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;

        }
        else
        {
            Debug.Log("Save file not found in " + path);
            return null;
        }
    }

    // Deletes the save file for good
    public static void DeletePlayer(string manualFileName)
    {
        // gets the path of the selected save file
        string path = Application.persistentDataPath + "/" + manualFileName + ".SL";

        // if it exists, deletes it
        if (File.Exists(path))
        {
            Debug.Log("File deleted");
            File.Delete(path);
        }
    }

    public static bool LoadedGame
    {
        get { return loadedGame; }
        set { loadedGame = value; }
    }

    public static string CurrentFileName
    {
        get { return currentFileName; }
        set { currentFileName = value; }
    }

    public static PlayerData CurrentPlayerData
    {
        get { return currentPlayerData; }
        set { currentPlayerData = value; }
    }

    #endregion
}
