using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawningRessources : MonoBehaviour
{
    [SerializeField] private GameObject _actualRessource;
    [SerializeField] private GameObject _rock, _wood, _meat;
    [SerializeField] private GameObject _ressourceCreate;
    [SerializeField] private GameObject _ressourceParent;
    private Spawning _spawning = new Spawning();
    private Vector2 _posToBuild;
    private bool _canBuild;
    public Tilemap tilemap;
    private int index;
    public Dictionary<string, string> ressourceList = new Dictionary<string, string>();
    private string _tagToAdd;


    // Start is called before the first frame update
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
            objectName = ressource.Key.Replace("1", "").Replace("2", "").Replace("3", "").Replace("4", "")
                .Replace("5", "").Replace("6", "").Replace("7", "").Replace("8", "").Replace("9", "").Replace("0", "");
            print(objectName);
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
                Quaternion.identity, _ressourceParent.transform);
            _ressourceCreate.tag = _tagToAdd;
        }

        _ressourceCreate = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (_actualRessource)
        {
            if (Input.GetMouseButton(0))
            {
                _posToBuild = _spawning.SpawnPosition(tilemap, out _canBuild);
                if (_canBuild)
                {
                    _ressourceCreate = Instantiate(_actualRessource, _posToBuild, Quaternion.identity);
                    _ressourceCreate.transform.SetParent(_ressourceParent.transform);
                    _ressourceCreate.name = _actualRessource.name + index;
                    _canBuild = false;
                    ressourceList.Add(_actualRessource.name + index, _posToBuild.ToString());
                    index++;
                    if (SaveAndLoad.Instance)
                    {
                        SaveAndLoad.Instance.SaveRessource(ressourceList, index);
                        SaveAndLoad.Instance.SaveGame();
                    }
                }
            }
        }
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