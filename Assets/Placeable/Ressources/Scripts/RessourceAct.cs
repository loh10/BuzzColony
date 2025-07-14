using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ERessource
{
    Nourriture,
    Roche,
    Bois,
    Null
}

public class RessourceAct : MonoBehaviour
{
    [SerializeField]private int _nbWood;
    [SerializeField]private int _nbStone;
    [SerializeField]private int _nbFood;
    [SerializeField]private int _nbClick = 10;
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
        SetMaxRessource();
    }

    public void AddRessource(int nbToAdd, ERessource eRessourceToAdd)
    {
        switch (eRessourceToAdd)
        {
            case ERessource.Nourriture:
                _nbFood += nbToAdd;
                break;
            case ERessource.Bois:
                _nbWood += nbToAdd;
                break;
            case ERessource.Roche:
                _nbStone += nbToAdd;
                break;
        }
        SetMaxRessource();
    }

    private void UpdateText()
    {
        woodText.text = $"{_nbWood}/{maxRessource}";
        stoneText.text = $"{_nbStone}/{maxRessource}";
        foodText.text = $"{_nbFood}/{maxRessource}";
        clickText.text = $"{_nbClick}/{maxRessource / 3}";
        if (_saveAndLoad)
        {
            _saveAndLoad.SaveRessourceAct(_nbWood, _nbStone, _nbFood, _nbClick);
        }
    }

    public void ResetClick()
    {
        _nbClick = maxRessource / 3;
        UpdateText();
    }

    private void SetMaxRessource()
    {
        maxRessource = reserveParent.childCount * 10 + 30;
        UpdateText();
    }
    
}