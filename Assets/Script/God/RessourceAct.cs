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
}

public class RessourceAct : MonoBehaviour
{
    public int nbWood;
    public int nbStone;
    public int nbFood;
    public int nbClick;
    public static RessourceAct Instance;
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI stoneText;
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI clickText;
    public int maxRessource;
    public Transform reserveParent;

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
        UpdateText();
    }

    private void Start()
    {
        if (SaveAndLoad.Instance)
        {
            nbWood = SaveAndLoad.Instance.nbWood;
            nbStone = SaveAndLoad.Instance.nbStone;
            nbFood = SaveAndLoad.Instance.nbFood;
            nbClick = SaveAndLoad.Instance.nbClick;
            print(SaveAndLoad.Instance.nbWood);
        }   
        UpdateText();
    }
    
    public int GetWood()
    {
        return nbWood;
    }
    
    public int GetStone()
    {
        return nbStone;
    }

    public int GetFood()
    {
        return nbFood;
    }

    public int GetClick()
    {
        return nbClick;
    }
    
    public void UseClick()
    {
        nbClick--;
        SaveAndLoad.Instance.SaveRessource(nbWood, nbStone, nbFood,nbClick);
        SaveAndLoad.Instance.SaveGame();
        UpdateText();
    }
    public void AddRessource(int nbToAdd, Ressource nameToAdd)
    {
        switch (nameToAdd)
        {
            case Ressource.Nourriture:
                nbFood += nbToAdd;
                break;
            case Ressource.Bois:
                nbWood += nbToAdd;
                break;
            case Ressource.Roche:
                nbStone += nbToAdd;
                break;
        }
        SaveAndLoad.Instance.SaveRessource(nbWood, nbStone, nbFood,nbClick);
        SaveAndLoad.Instance.SaveGame();
        UpdateText();
    }

    private void UpdateText()
    {
        maxRessource = SetMaxRessource();
        woodText.text = $"{nbWood}/{maxRessource}";
        stoneText.text = $"{nbStone}/{maxRessource}";
        foodText.text = $"{nbFood}/{maxRessource}";
        clickText.text = $"{nbClick}/{maxRessource/3}";
    }
    
    private int SetMaxRessource()
    {   
        return reserveParent.childCount*10+30;
    }
}
