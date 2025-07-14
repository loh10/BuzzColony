using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MessageBtn : MonoBehaviour
{
    private int nb;
    private ERessource _eRessource;
    private int index;

    public void GetValue(int nbToAdd, ERessource nameToAdd)
    {
        nb = nbToAdd;
        _eRessource = nameToAdd;
    }

    public void Recruter()
    {
        index = transform.parent.transform.GetSiblingIndex();
        Settler[] allChildren= GameObject.Find("ColonParent").GetComponentsInChildren<Settler>();
        if(CheckRessources(allChildren[index].nbRessources, RessourceAct.Instance.GetWood(), RessourceAct.Instance.GetStone(),
               RessourceAct.Instance.GetFood(), allChildren[index].nameERessource))
        {
            RessourceAct.Instance.AddRessource(-allChildren[index].nbRessources, allChildren[index].nameERessource);
            allChildren[index].isMine = true;
            allChildren[index].transform.parent = GameObject.Find("ColonMine").transform;
            if (SaveAndLoad.Instance)
            {
                SaveAndLoad.Instance.SaveColon(index, allChildren[index].transform.position.ToString());
            }
            Destroy(gameObject.transform.parent.gameObject);
        } 
    }


    public bool CheckRessources(int nbNeed, int nbWood, int nbStone, int nbFood, ERessource eRessource)
    {
        switch (eRessource)
        {
            case ERessource.Nourriture:
                if (nbFood >= nb)
                {
                    return true;
                }
                break;
            case ERessource.Bois:
                if (nbWood >= nb)
                {
                    return true;
                }
                break;
            case ERessource.Roche:
                if (nbStone >= nb)
                {
                    return true;
                }
                break;
        }

        return false;
    }
}