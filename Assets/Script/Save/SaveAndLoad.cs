using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameData
{
    // Map
    public int SEED;

    // Construction
    public string CONSTRUCTIONNAME;
    public string CONSTRUCTIONPOSITION;
    public int NBCONSTRUCTION;

    // Resources
    public string MEATNAME;
    public string MEATPOSITION;
    public int NBMEAT;

    public string ROCKNAME;
    public string ROCKPOSITION;
    public int NBROCK;

    public string WOODNAME;
    public string WOODPOSITION;
    public int NBWOOD;
}
public class SaveAndLoad : MonoBehaviour
{
    public static SaveAndLoad Instance;
    private GameData _gameData;
    private string _saveFilePath;

    // Map
    public int _seed;

    // Construction
    public Dictionary<string, string> _construction;
    public int _nbConstruction;

    // Resources
    public Dictionary<string, string> _meat;
    public int _nbMeat;

    public Dictionary<string, string> _rock;
    public int _nbRock;

    public Dictionary<string, string> _wood;
    public int _nbWood;

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

    public void SaveConstruction(Dictionary<string, string> construction, int nbConstruction)
    {
        _construction = construction;
        _nbConstruction = nbConstruction;
    }

    public void SaveMeat(Dictionary<string, string> meat, int nbMeat)
    {
        _meat = meat;
        _nbMeat = nbMeat;
    }

    public void SaveRock(Dictionary<string, string> rock, int nbRock)
    {
        _rock = rock;
        _nbRock = nbRock;
    }

    public void SaveWood(Dictionary<string, string> wood, int nbWood)
    {
        _wood = wood;
        _nbWood = nbWood;
    }

    private void SaveInformation()
    {
        _gameData.SEED = _seed;

        // Construction
        if (_construction != null)
        {
            _gameData.CONSTRUCTIONNAME = "";
            _gameData.CONSTRUCTIONPOSITION = "";
            foreach (string name in _construction.Keys)
            {
                _gameData.CONSTRUCTIONNAME += name + ",";
                _gameData.CONSTRUCTIONPOSITION += _construction[name] + ";";
            }
            _gameData.NBCONSTRUCTION = _nbConstruction;
        }

        // Meat
        if (_meat != null)
        {
            _gameData.MEATNAME = "";
            _gameData.MEATPOSITION = "";
            foreach (string name in _meat.Keys)
            {
                _gameData.MEATNAME += name + ",";
                _gameData.MEATPOSITION += _meat[name] + ";";
            }
            _gameData.NBMEAT = _nbMeat;
        }

        // Rock
        if (_rock != null)
        {
            _gameData.ROCKNAME = "";
            _gameData.ROCKPOSITION = "";
            foreach (string name in _rock.Keys)
            {
                _gameData.ROCKNAME += name + ",";
                _gameData.ROCKPOSITION += _rock[name] + ";";
            }
            _gameData.NBROCK = _nbRock;
        }

        // Wood
        if (_wood != null)
        {
            _gameData.WOODNAME = "";
            _gameData.WOODPOSITION = "";
            foreach (string name in _wood.Keys)
            {
                _gameData.WOODNAME += name + ",";
                _gameData.WOODPOSITION += _wood[name] + ";";
            }
            _gameData.NBWOOD = _nbWood;
        }
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
            string loadPlayerData = File.ReadAllText(_saveFilePath);
            _gameData = JsonUtility.FromJson<GameData>(loadPlayerData);
            _seed = _gameData.SEED;
            SetConstruction();
            _nbConstruction = _gameData.NBCONSTRUCTION;
            SetMeat();
            _nbMeat = _gameData.NBMEAT;
            SetRock();
            _nbRock = _gameData.NBROCK;
            SetWood();
            _nbWood = _gameData.NBWOOD;
        }
    }

    private void SetConstruction()
    {
        string[] nameConstruction = _gameData.CONSTRUCTIONNAME.Split(",");
        string[] positionConstruction = _gameData.CONSTRUCTIONPOSITION.Split(";");
        _construction = new Dictionary<string, string>();
        for (int i = 0; i < nameConstruction.Length - 1; i++)
        {
            _construction.Add(nameConstruction[i], positionConstruction[i]);
        }
    }

    private void SetMeat()
    {
        string[] nameMeat = _gameData.MEATNAME.Split(",");
        string[] positionMeat = _gameData.MEATPOSITION.Split(";");
        _meat = new Dictionary<string, string>();
        for (int i = 0; i < nameMeat.Length - 1; i++)
        {
            _meat.Add(nameMeat[i], positionMeat[i]);
        }
    }

    private void SetRock()
    {
        string[] nameRock = _gameData.ROCKNAME.Split(",");
        string[] positionRock = _gameData.ROCKPOSITION.Split(";");
        _rock = new Dictionary<string, string>();
        for (int i = 0; i < nameRock.Length - 1; i++)
        {
            _rock.Add(nameRock[i], positionRock[i]);
        }
    }

    private void SetWood()
    {
        string[] nameWood = _gameData.WOODNAME.Split(",");
        string[] positionWood = _gameData.WOODPOSITION.Split(";");
        _wood = new Dictionary<string, string>();
        for (int i = 0; i < nameWood.Length - 1; i++)
        {
            _wood.Add(nameWood[i], positionWood[i]);
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