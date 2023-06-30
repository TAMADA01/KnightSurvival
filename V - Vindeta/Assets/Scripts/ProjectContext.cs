using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.Collections;
using UnityEngine;

public class ProjectContext : MonoBehaviour
{

    public int Record = 0;
    public int Wave = 0;
    public int EnemyCount = 0;
    public PauseManager PauseManager { get; private set; }
    public InputData InputData { get; private set; }

    public static ProjectContext Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        LoadGame();
        Destroy(gameObject);
    }

    public void Initialized()
    {
        PauseManager = new PauseManager();
        InputData = new InputData();
        LoadGame();
    }

    public void SaveGame()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = File.Create(Application.persistentDataPath + "/MySaveData.dat");
        SaveData saveData = new SaveData();

        Record = Wave > Record ? Wave : Record;
        saveData.Record = Record;

        binaryFormatter.Serialize(fileStream, saveData);
        fileStream.Close();
    }

    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/MySaveData.dat"))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = File.Open(Application.persistentDataPath + "/MySaveData.dat", FileMode.Open);
            SaveData saveData = (SaveData)binaryFormatter.Deserialize(fileStream);
            fileStream.Close();
            Record = saveData.Record;
        }
    }

    public void ReducingEnemy()
    {
        EnemyCount--;
    }
}

[Serializable]
public class SaveData
{
    public int Record;
}
