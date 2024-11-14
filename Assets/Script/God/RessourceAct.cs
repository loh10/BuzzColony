using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum Ressource
{
    Nourriture,
    Roche,
    Bois,
    Null
}

public class RessourceAct : MonoBehaviour
{
    private int _nbWood;
    private int _nbStone;
    private int _nbFood;
    private int _nbClick;
    public static RessourceAct Instance { get; private set; }
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI stoneText;
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI clickText;
    public int maxRessource { get; private set; }
    public Transform reserveParent;
    private SaveAndLoad _saveAndLoad;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _saveAndLoad = SaveAndLoad.Instance;
        SetMaxRessource();
    }

    private void Start()
    {
        if (_saveAndLoad)
        {
            _nbWood = _saveAndLoad.nbWood;
            _nbStone = _saveAndLoad.nbStone;
            _nbFood = _saveAndLoad.nbFood;
            _nbClick = _saveAndLoad.nbClick;
        }

        SetMaxRessource();
    }

    public int GetWood()
    {
        return _nbWood;
    }

    public int GetStone()
    {
        return _nbStone;
    }

    public int GetFood()
    {
        return _nbFood;
    }

    public int GetClick()
    {
        return _nbClick;
    }

    public void UseClick()
    {
        _nbClick--;
        if (_saveAndLoad)
        {
            _saveAndLoad.SaveRessourceAct(_nbWood, _nbStone, _nbFood, _nbClick);
        }

        SetMaxRessource();
    }

    public void AddRessource(int nbToAdd, Ressource ressourceToAdd)
    {
        switch (ressourceToAdd)
        {
            case Ressource.Nourriture:
                _nbFood += nbToAdd;
                break;
            case Ressource.Bois:
                _nbWood += nbToAdd;
                break;
            case Ressource.Roche:
                _nbStone += nbToAdd;
                break;
        }

        if (_saveAndLoad)
        {
            _saveAndLoad.SaveRessourceAct(_nbWood, _nbStone, _nbFood, _nbClick);
        }

        SetMaxRessource();
    }

    private void UpdateText()
    {
        woodText.text = $"{_nbWood}/{maxRessource}";
        stoneText.text = $"{_nbStone}/{maxRessource}";
        foodText.text = $"{_nbFood}/{maxRessource}";
        clickText.text = $"{_nbClick}/{maxRessource / 3}";
    }

    public void ResetClick()
    {
        _nbClick = maxRessource / 3;
        UpdateText();
    }

    private void SetMaxRessource()
    {
        UpdateText();
        maxRessource = reserveParent.childCount * 10 + 30;
    }
    
    private void OnGUI()
    {
        if (GUILayout.Button("AddRessource"))
        {
            AddRessource(maxRessource, Ressource.Bois);
            AddRessource(maxRessource, Ressource.Roche);
            AddRessource(maxRessource, Ressource.Nourriture);
        }
    }
}