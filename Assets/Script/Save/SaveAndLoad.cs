using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameData
{
    //Map
    public int SEED;
}
public class SaveAndLoad : MonoBehaviour
{
    public static SaveAndLoad Instance;
    private GameData _gameData;
    private string _saveFilePath;
    
    //Map
    public int _seed;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance == null)
        {
            Instance = this;
            _gameData = new GameData();
            _saveFilePath = Application.dataPath + "/SaveData.json";
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveMap(int seed)
    {
        _seed = seed;
    }

    private void SaveInformation()
    {
        _gameData.SEED = _seed;
    }
    public void SaveGame()
    {
        SaveInformation();
        string savePlayerData = JsonUtility.ToJson(_gameData);
        File.WriteAllText(_saveFilePath,savePlayerData);
    }

    public void LoadGame()
    {
        if (File.Exists(_saveFilePath))
        {
            string loadPlayerData = File.ReadAllText((_saveFilePath));
            _gameData = JsonUtility.FromJson<GameData>(loadPlayerData);
            _seed = _gameData.SEED;
        }
    }
    
    public void DeleteSaveFile()
    {
        if (File.Exists(_saveFilePath))
        {
            File.Delete(_saveFilePath);
        }
    }
}
