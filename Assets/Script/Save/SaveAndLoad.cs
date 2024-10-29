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
    public string RESSOURCENAME;
    public string RESSOURCEPOSITION;
    public int NBRESSOURCE;
    public int WOODNB;
    public int STONENB;
    public int FOODNB;
    public int CLICKNB;


    //Messages
    public string COLONNAME;
    public string COLONPOSITION;
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
    public Dictionary<string, string> _ressource;
    public int _nbRessource;
    public int nbWood;
    public int nbStone;
    public int nbFood;
    public int nbClick;

    //Messages
    public Dictionary<int, string> _colon;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _gameData = new GameData();
            _saveFilePath = Application.dataPath + "/SaveData.json";
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        //LoadGame();
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

    public void SaveRessource(Dictionary<string, string> ressource, int nbRessource)
    {
        _ressource = ressource;
        _nbRessource = nbRessource;
    }

    public void SaveColon(int index, string position)
    {
        _colon.Add(index, position);
    }

    public void SaveRessource(int Wood, int Stone, int Food,int Click)
    {
        nbWood = Wood;
        nbStone = Stone;
        nbFood = Food;
        nbClick = Click;
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

        // Resources
        if (_ressource != null)
        {
            _gameData.RESSOURCENAME = "";
            _gameData.RESSOURCEPOSITION = "";
            foreach (string name in _ressource.Keys)
            {
                _gameData.RESSOURCENAME += name + ",";
                _gameData.RESSOURCEPOSITION += _ressource[name] + ";";
            }

            _gameData.NBRESSOURCE = _nbRessource;
        }

        //Colon
        if (_colon != null)
        {
            _gameData.COLONNAME = "";
            _gameData.COLONPOSITION = "";
            foreach (int index in _colon.Keys)
            {
                _gameData.COLONNAME += index + ",";
                _gameData.COLONPOSITION += _colon[index] + ";";
            }
        }

        //Ressource
        _gameData.WOODNB = nbWood;
        _gameData.STONENB = nbStone;
        _gameData.FOODNB = nbFood;
    }

    public void SaveGame()
    {
        SaveInformation();
        if(!File.Exists(_saveFilePath))
        {
            ResetGameData();
        }
        string savePlayerData = JsonUtility.ToJson(_gameData);
        File.WriteAllText(_saveFilePath, savePlayerData);
    }

    private void ResetGameData()
    {
        nbFood = 15;
        nbStone = 15;
        nbWood = 15;
        nbClick = 10;
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
            SetRessource();
            _nbRessource = _gameData.NBCONSTRUCTION;
            SetColon();
            SetRessourceNb();
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

    private void SetRessource()
    {
        string[] nameConstruction = _gameData.RESSOURCENAME.Split(",");
        string[] positionConstruction = _gameData.RESSOURCEPOSITION.Split(";");
        _ressource = new Dictionary<string, string>();
        for (int i = 0; i < nameConstruction.Length - 1; i++)
        {
            _ressource.Add(nameConstruction[i], positionConstruction[i]);
        }
    }

    private void SetColon()
    {
        string[] colonName = _gameData.COLONNAME.Split(",");
        string[] colonPosition = _gameData.COLONPOSITION.Split(";");
        _colon = new Dictionary<int, string>();
        for (int i = 0; i < colonName.Length - 1; i++)
        {
            _colon.Add(int.Parse(colonName[i]), colonPosition[i]);
        }
    }

    private void SetRessourceNb()
    {
        nbWood = _gameData.WOODNB;
        nbStone = _gameData.STONENB;
        nbFood = _gameData.FOODNB;
        nbClick = _gameData.CLICKNB;
    }

    public void DeleteSaveFile()
    {
        if (File.Exists(_saveFilePath))
        {
            File.Delete(_saveFilePath);
            print($"file was deleted  here {_saveFilePath}");
        }
    }
}