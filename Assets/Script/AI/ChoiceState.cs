using System;
using System.Collections.Generic;
using System.Linq;
using MyUtils;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public enum ActTask
{
    Collect,
    Build,
    Fight,
    Nothing
};

public class ChoiceState : MonoBehaviour
{
    public Agent agent;
    public AStarPathfinder aStarPathfinder;

    public Vector2Int pathStart;
    public Vector2Int pathGoal;
    public bool randomPath;

    public ActTask actTask = ActTask.Nothing;
    public bool isWorking = false;
    private Colon colon;

    #region Ressource

    private RessourceAct _ressourceAct;
    [SerializeField] private int _ressourceNb;
    [SerializeField] private string _ressourceMin;

    Transform woodTransform;
    Transform rockTransform;
    Transform meatTransform;

    #endregion

    #region Ennemi

    [Header("Ennemi")] [SerializeField] private Transform _ennemiParent;
    [SerializeField] private float detectRange;

    [Space(2)]

    #endregion

    #region Construction

    [Header("Construction")]
    [SerializeField]
    private Transform _constructionParent;

    private List<Construction> _constructionInProgresse = new List<Construction>();
    private GameObject _nearestConstruction;

    #endregion


    void Start()
    {
        woodTransform = GameObject.Find("Wood").transform;
        meatTransform = GameObject.Find("Meat").transform;
        rockTransform = GameObject.Find("Rock").transform;
        agent = GetComponent<Agent>();
        _constructionParent = GameObject.Find("Construction").transform;
        _ennemiParent = GameObject.Find("Ennemi").transform;
        _ressourceAct = RessourceAct.Instance;
        aStarPathfinder = GetComponent<AStarPathfinder>();
        colon = GetComponent<Colon>();
    }

    void Update()
    {
        if (!isWorking && colon.isMine)
        {
            if (CheckEnnemi()) //check if ennemy near
            {
                actTask = ActTask.Fight;
            }
            else if (CheckConstrucionInProgresse()) //Check si construction a faire
            {
                actTask = ActTask.Build;
                isWorking = true;
                CheckNearestConstruction();
            }
            else if (CheckRessource()) //check ressource
            {
                isWorking = true;
                actTask = ActTask.Collect;
                RecoltRessource();
            }
            else //balade
            {
                actTask = ActTask.Nothing;
                isWorking = false;
            }
        }
    }

    #region Ennemi

    private bool CheckEnnemi()
    {
        foreach (Transform ennemi in _ennemiParent)
        {
            if (Vector3.Distance(agent.transform.position, ennemi.position) < detectRange)
            {
                return true;
            }
        }

        return false;
    }

    #endregion

    #region Construction

    private bool CheckConstrucionInProgresse()
    {
        foreach (Construction construction in _constructionParent.GetComponentsInChildren<Construction>())
        {
            if (construction.isBuild == false)
            {
                _constructionInProgresse.Add(construction);
                return true;
            }
        }

        return false;
    }

    private void CheckNearestConstruction()
    {
        GameObject nearestConstruction = null;
        float distance = float.PositiveInfinity;
        foreach (Construction construction in _constructionInProgresse)
        {
            if (Vector3.Distance(agent.transform.position, construction.transform.position) < distance)
            {
                nearestConstruction = construction.gameObject;
                distance = Vector3.Distance(agent.transform.position, construction.transform.position);
            }
        }

        _nearestConstruction = nearestConstruction;
        GoToRessource(new Vector2Int((int)transform.position.x, (int)transform.position.y),
            new Vector2Int((int)nearestConstruction.transform.position.x,
                (int)nearestConstruction.transform.position.y));
    }

    #endregion

    #region Ressource

    private bool CheckRessource()
    {
        if ((_ressourceAct.GetWood() < _ressourceAct.maxRessource ||
             _ressourceAct.GetStone() < _ressourceAct.maxRessource ||
             _ressourceAct.GetFood() < _ressourceAct.maxRessource))
        {
            return true;
        }

        return false;
    }

    private void RecoltRessource()
    {
        Dictionary<string, int> actualRessource = new Dictionary<string, int>()
        {
            { "Wood", _ressourceAct.GetWood() }, { "Rock", _ressourceAct.GetStone() },
            { "Meat", _ressourceAct.GetFood() }
        };
        GetLessRessource(actualRessource);
        print($"{_ressourceNb+1} et {_ressourceAct.maxRessource}");
        if (_ressourceNb+1 < _ressourceAct.maxRessource && _ressourceNb > 0)
        {
            isWorking = true;
            ChooseRessource(_ressourceMin);
        }
        else
        {
            isWorking = false;
        }
    }

    void GetLessRessource(Dictionary<string, int> ressource)
    {
        _ressourceNb = 0;
        var minRessource = ressource.OrderBy(_ressource => _ressource.Value);
        _ressourceMin = minRessource.First().Key;
        //check bois plus petit
        while (_ressourceNb == 0)
        {
            _ressourceNb = woodTransform.childCount;
            _ressourceMin = "Wood";
            if (_ressourceNb == 0 || _ressourceAct.GetWood()+ _ressourceNb >= _ressourceAct.maxRessource)
            {
                _ressourceNb = rockTransform.childCount;
                _ressourceMin = "Rock";
                if (_ressourceNb == 0|| _ressourceAct.GetStone()+ _ressourceNb >= _ressourceAct.maxRessource)
                {
                    _ressourceNb = meatTransform.childCount;
                    _ressourceMin = "Meat";
                    if (_ressourceNb == 0|| _ressourceAct.GetFood()+ _ressourceNb >= _ressourceAct.maxRessource)
                    {
                        _ressourceNb = -1;
                        isWorking = false;
                        return;
                    }
                }
            }
        }
    }

    void ChooseRessource(string indexRessource)
    {
        Transform[] ressourceTransform = null;
        GameObject[] ressource = null;


        switch (indexRessource)
        {
            case "Wood": //Wood
                ressourceTransform = woodTransform.transform.GetComponentsInChildren<Transform>();
                break;
            case "Rock": //Rock
                ressourceTransform = rockTransform.transform.GetComponentsInChildren<Transform>();
                break;
            case "Meat": //Meat
                ressourceTransform = meatTransform.transform.GetComponentsInChildren<Transform>();
                break;
        }

        if (ressourceTransform != null)
        {
            ressource = ressourceTransform.Select(t => t.gameObject).ToArray();
            ressource = Utils.RemoveFirstIndex(ressource);
            if (ressource.Length != 0)
            {
                GameObject nearestRessource = GetClosestRessource(ressource);
                GoToRessource(new Vector2Int((int)transform.position.x, (int)transform.position.y),
                    new Vector2Int((int)nearestRessource.transform.position.x,
                        (int)nearestRessource.transform.position.y));
                return;
            }

            isWorking = false;
        }
    }

    private GameObject GetClosestRessource(GameObject[] ressource)
    {
        GameObject nearestProduct;
        nearestProduct = ressource[0] ?? new GameObject();
        foreach (GameObject res in ressource)
        {
            if (Vector3.Distance(this.transform.position, res.transform.position) <
                Vector3.Distance(this.transform.position, nearestProduct.transform.position))
            {
                nearestProduct = res;
            }
        }

        return nearestProduct;
    }

    #endregion

    private void GoToRessource(Vector2Int x, Vector2Int y)
    {
        aStarPathfinder.FindPath(x, y, agent);
        isWorking = true;
    }
}