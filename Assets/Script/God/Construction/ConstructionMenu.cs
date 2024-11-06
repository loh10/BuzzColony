using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.Globalization;


public class ConstructionMenu : MonoBehaviour
{
    private bool isActive;
    [SerializeField] private GameObject _marketMenu, _currentConstruction;
    [SerializeField] private SpawningMenu _spawningMenu;
    [SerializeField] private Transform _constructionParent;
    public Tilemap tilemap;
    private Vector3 constructionPosition;
    private bool canBuild;
    private new Camera camera;
    private Vector3 worldPoint;
    private Vector3Int cellPosition;
    private Vector3 cellCenterPosition;
    private Vector2 _size;
    private string _tagToAdd;
    private int _nbConstruction;
    [SerializeField] private ConstructionSO _habitation, _reserve, _champs, _zoneAtterissage, _currentConstructionSO;
    [SerializeField] private Button _habitationButton, _reserveButton, _champsButton, _zoneAtterissageButton;
    private RessourceAct ressourceActuel;


    public Dictionary<string, string> constructionList = new Dictionary<string, string>();

    private void Start()
    {
        _marketMenu.SetActive(false);
        if (SaveAndLoad.Instance != null)
        {
            if (SaveAndLoad.Instance._construction != null)
            {
                constructionList = SaveAndLoad.Instance._construction;
                _nbConstruction = SaveAndLoad.Instance._nbConstruction;

                BuildAllBatiment();
            }
        }
    }

    private void BuildAllBatiment()
    {
        foreach (var construction in constructionList)
        {
            GameObject objectToConstruct = null;
            string objectName;
            objectName = construction.Key.Replace("1", "").Replace("2", "").Replace("3", "").Replace("4", "")
                .Replace("5", "").Replace("6", "").Replace("7", "").Replace("8", "").Replace("9", "").Replace("0", "");
            if (objectName == "House")
            {
                objectToConstruct = _habitation.constructionPrefab;
                _size = _habitation.constructionSize;
                _tagToAdd = _habitation.tag;
            }

            if (objectName == "Storage")
            {
                objectToConstruct = _reserve.constructionPrefab;
                _size = _reserve.constructionSize;
                _tagToAdd = _reserve.tag;
            }

            if (objectName == "Field")
            {
                objectToConstruct = _champs.constructionPrefab;
                _size = _champs.constructionSize;
                _tagToAdd = _champs.tag;
            }

            if (objectName == "LandingZone")
            {
                objectToConstruct = _zoneAtterissage.constructionPrefab;
                _size = _zoneAtterissage.constructionSize;
                _tagToAdd = _zoneAtterissage.tag;
            }

            _currentConstruction = Instantiate(objectToConstruct, StringToVector2(construction.Value),
                Quaternion.identity, _constructionParent);
        }

        _currentConstruction = null;
    }

    private void Update()
    {
        if (_currentConstruction != null)
        {
            GetCellCenterPosition();
            _currentConstruction.transform.position = cellCenterPosition;
            if (Input.GetMouseButtonDown(0))
            {
                constructionPosition = SpawnPosition(tilemap, out canBuild, _size.x, _size.y);
                if (canBuild)
                {
                     RessourceAct.Instance.AddRessource(-_currentConstructionSO.constructionCost[0], Ressource.Bois);
                     RessourceAct.Instance.AddRessource(-_currentConstructionSO.constructionCost[1], Ressource.Roche);
                     RessourceAct.Instance.AddRessource(-_currentConstructionSO.constructionCost[2], Ressource.Nourriture);
                    _currentConstruction.tag = _tagToAdd;
                    BuildConstruction(_currentConstruction);
                }
            }
        }
    }

    private bool CheckRessource(int[] ressourceDemande)
    {
        ressourceActuel = RessourceAct.Instance;
        for (int i = 0; i < ressourceDemande.Length; i++)
        {
            switch (i)
            {
                case 0:
                    if (ressourceActuel.GetWood() < ressourceDemande[i])
                        return false;
                    break;

                case 1:
                    if (ressourceActuel.GetStone() < ressourceDemande[i])
                        return false;
                    break;

                case 2:
                    if (ressourceActuel.GetFood() < ressourceDemande[i])
                        return false;
                    break;
            }
        }

        return true;
    }

    private void BuildConstruction( GameObject _construction)
    {
        SetParent(_currentConstruction, _tagToAdd);
        _currentConstruction = null;
        _nbConstruction++;
        constructionList.Add(_tagToAdd + _nbConstruction, constructionPosition.ToString());
        if (SaveAndLoad.Instance)       
        {
            SaveAndLoad.Instance.SaveConstruction(constructionList, _nbConstruction);
            SaveAndLoad.Instance.SaveGame();
        }
        _construction.GetComponent<Construction>().isPlace = true;
    }

    private Vector2 StringToVector2(string value)
    {
        value = value.Replace("(", "").Replace(")", "");
        string[] vector = value.Split(',');
        Vector2 vector2 = new Vector2(float.Parse(vector[0], CultureInfo.InvariantCulture.NumberFormat),
            float.Parse(vector[1], CultureInfo.InvariantCulture.NumberFormat));
        return vector2;
    }

    private void GetCellCenterPosition()
    {
        camera = Camera.main;
        if (camera != null) worldPoint = camera.ScreenToWorldPoint(Input.mousePosition);
        cellPosition = tilemap.WorldToCell(worldPoint);
        cellCenterPosition = tilemap.GetCellCenterWorld(cellPosition);
    }

    private Vector3 SpawnPosition(Tilemap tilemap, out bool _canBuild, float sizeX, float sizeY)
    {
        GetCellCenterPosition();
        bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
        if (tilemap.HasTile(cellPosition) && !isOverUI)
        {
            // Check if there is already wood on this tile
            Collider2D[] colliders = Physics2D.OverlapAreaAll(cellCenterPosition,
                new Vector2(cellCenterPosition.x + sizeX, cellCenterPosition.y + sizeY));
            foreach (var collider in colliders)
            {
                GameObject collision = collider.gameObject;
                if (collision.CompareTag("Wood") || collision.CompareTag("Rock") || collision.CompareTag("Mountain") ||
                    collision.CompareTag("Meat") || collision.CompareTag("House") || collision.CompareTag("Storage") ||
                    collision.CompareTag("Field") || collision.CompareTag("LandingZone"))
                {
                    _canBuild = false;
                    return Vector3.zero;
                }
            }

            _canBuild = true;
            return cellCenterPosition;
        }

        _canBuild = false;
        return Vector3.zero;
    }

    public void ActiveMenu()
    {
        isActive = !isActive;
        _marketMenu.SetActive(isActive);
        _spawningMenu.DisableAllRessource();
        CheckButton();
    }

    private void CheckButton()
    {
        if (CheckRessource(_habitation.constructionCost))
        {
            _habitationButton.interactable = true;
        }
        else
        {
            _habitationButton.interactable = false;
        }

        if (CheckRessource(_reserve.constructionCost))
        {
            _reserveButton.interactable = true;
        }
        else
        {
            _reserveButton.interactable = false;
        }

        if (CheckRessource(_champs.constructionCost))
        {
            _champsButton.interactable = true;
        }
        else
        {
            _champsButton.interactable = false;
        }

        if (CheckRessource(_zoneAtterissage.constructionCost))
        {
            _zoneAtterissageButton.interactable = true;
        }
        else
        {
            _zoneAtterissageButton.interactable = false;
        }
    }

    public void CloseConstructionMenu()
    {
        isActive = false;
        _marketMenu.SetActive(isActive);
    }

    public void SetConstruction(ConstructionSO constructionType)
    {
        Vector3 mousePosition = Input.mousePosition;
        if (Camera.main != null)
        {
            mousePosition.z = Camera.main.nearClipPlane;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            _currentConstruction = Instantiate(constructionType.constructionPrefab, worldPosition, Quaternion.identity);
            _size = constructionType.constructionSize;
            _tagToAdd = constructionType.tag;
            _currentConstructionSO = constructionType;
        }
    }

    private void SetParent(GameObject _construction, string tag)
    {
        switch (tag)
        {
            case "House":
                _construction.transform.SetParent(_constructionParent.GetChild(0));
                break;
            case "Storage":
                _construction.transform.SetParent(_constructionParent.GetChild(1));
                break;
            case "Field":
                _construction.transform.SetParent(_constructionParent.GetChild(2));
                break;
            default:
                _construction.transform.SetParent(_constructionParent);
                break;
        }
    }
}