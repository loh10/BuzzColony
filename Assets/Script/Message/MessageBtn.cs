using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MessageBtn : MonoBehaviour
{
    private int nb;
    private Ressource ressource;
    private int index;

    public void GetValue(int nbToAdd, Ressource nameToAdd)
    {
        nb = nbToAdd;
        ressource = nameToAdd;
    }

    public void Recruter()
    {
        index = transform.parent.transform.GetSiblingIndex();
        Colon[] allChildren= GameObject.Find("ColonParent").GetComponentsInChildren<Colon>();
        if(CheckRessources(allChildren[index].nbRessources, RessourceAct.Instance.GetWood(), RessourceAct.Instance.GetStone(),
               RessourceAct.Instance.GetFood(), allChildren[index].nameRessource))
        {
            RessourceAct.Instance.AddRessource(-allChildren[index].nbRessources, allChildren[index].nameRessource);
            allChildren[index].isMine = true;
            allChildren[index].transform.parent = GameObject.Find("ColonMine").transform;
            Destroy(gameObject.transform.parent.gameObject);
        }
    }


    public bool CheckRessources(int nbNeed, int nbWood, int nbStone, int nbFood, Ressource ressource)
    {
        switch (ressource)
        {
            case Ressource.Nourriture:
                if (nbFood >= nb)
                {
                    return true;
                }
                break;
            case Ressource.Bois:
                if (nbWood >= nb)
                {
                    return true;
                }
                break;
            case Ressource.Roche:
                if (nbStone >= nb)
                {
                    return true;
                }
                break;
        }

        return false;
    }
}