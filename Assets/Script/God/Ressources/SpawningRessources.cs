using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using MyUtils;
public class SpawningRessources : MonoBehaviour
{
    [SerializeField] private GameObject _actualRessource;
    [SerializeField] private GameObject _rock, _wood, _meat;
    [SerializeField] private GameObject _ressourceCreate;
    private Spawning _spawning = new Spawning();
    private Vector2 _posToBuild;
    private bool _canBuild;
    public Tilemap tilemap;
    private int index;
    public Dictionary<string, string> ressourceList = new Dictionary<string, string>();
    private string _tagToAdd;

    void Start()
    {
        if (SaveAndLoad.Instance != null)
        {
            index = SaveAndLoad.Instance._nbRessource;

            if (index > 0)
            {
                ressourceList = SaveAndLoad.Instance._ressource;
                PlaceAllRessource();
            }

            index++;
        }
    }

    private void PlaceAllRessource()
    {
        foreach (var ressource in ressourceList)
        {
            GameObject objectToConstruct = null;
            string objectName;
            objectName = Utils.NumberRemover(ressource.Key);
            if (objectName == "Wood")
            {
                objectToConstruct = _wood;
                _tagToAdd = "Wood";
            }
            else if (objectName == "Rock")
            {
                objectToConstruct = _rock;
                _tagToAdd = "Rock";
            }
            else if (objectName == "Meat")
            {
                objectToConstruct = _meat;
                _tagToAdd = "Meat";
            }

            _ressourceCreate = Instantiate(objectToConstruct, _spawning.StringToVector2(ressource.Value),
                Quaternion.identity, this.transform);
            SetParent(objectName,_ressourceCreate);

            _ressourceCreate.tag = _tagToAdd;
        }

        _ressourceCreate = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (_actualRessource)
        {
            if (Input.GetMouseButtonDown(0) && RessourceAct.Instance.GetClick() > 0)
            {
                _posToBuild = _spawning.SpawnPosition(tilemap, out _canBuild);
                if (_canBuild)
                {
                    _ressourceCreate = Instantiate(_actualRessource, _posToBuild, Quaternion.identity);
                    SetParent(_actualRessource.name,_ressourceCreate);
                    _ressourceCreate.name = _actualRessource.name + index;
                    _canBuild = false;
                    ressourceList.Add(_actualRessource.name + index, _posToBuild.ToString());
                    index++;
                    if (SaveAndLoad.Instance)
                    {
                        SaveAndLoad.Instance.SaveRessource(ressourceList, index);
                        SaveAndLoad.Instance.SaveGame();
                    }

                    RessourceAct.Instance.UseClick();
                }
            }
        }
    }

    private void SetParent(string ressource,GameObject ressourceObject)
    {
        switch (ressource)
        {
            case "Wood":
                ressourceObject.transform.SetParent(this.transform.GetChild(0));
                break;
            case "Rock":
                ressourceObject.transform.SetParent(this.transform.GetChild(1));
                break;
            case "Meat":
                ressourceObject.transform.SetParent(this.transform.GetChild(2));
                break;
        }
    }
    public void UnselectAll()
    {
        _actualRessource = null;
    }

    public void ChooseWood()
    {
        _actualRessource = _wood;
    }

    public void ChooseRock()
    {
        _actualRessource = _rock;
    }

    public void ChooseMeat()
    {
        _actualRessource = _meat;
    }
}