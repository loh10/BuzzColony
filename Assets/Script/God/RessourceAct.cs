using System;
using System.Collections;
using System.Collections.Generic;
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
    public static RessourceAct Instance;

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
    }

    public RessourceAct(int nbWood, int nbStone, int nbFood)
    {
        this.nbWood = nbWood;
        this.nbStone = nbStone;
        this.nbFood = nbFood;
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
    }
}
