using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameData
{
    //Map
    public int SEED;
    //Construction
    public Dictionary<string,Vector2> construction = new Dictionary<string, Vector2>();
}

public class SaveAndLoad : MonoBehaviour
{
    public static SaveAndLoad Instance;
    private GameData _gameData;
    private string _saveFilePath;

    //Map
    public int _seed;
    //Construction
    public Dictionary<string,Vector2> _construction = new Dictionary<string, Vector2>();
    
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
    public void SaveConstruction(Dictionary<string,Vector2> construction)
    {
        _construction = construction;
    }
    private void SaveInformation()
    {
        _gameData.SEED = _seed;
        _gameData.construction = _construction;
    }
    
    public void SaveGame()
    {
        SaveInformation();
        string savePlayerData = JsonUtility.ToJson(_gameData);
        File.WriteAllText(_saveFilePath, savePlayerData);
    }

    public void LoadGame()
    {
        if (File.Exists(_saveFilePath))
        {
            string loadPlayerData = File.ReadAllText((_saveFilePath));
            _gameData = JsonUtility.FromJson<GameData>(loadPlayerData);
            _seed = _gameData.SEED;
            _construction = _gameData.construction;
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