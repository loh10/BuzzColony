using System.Collections.Generic;
using System.Linq;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class ChoiceState : MonoBehaviour
{
    public Agent agent;
    public AStarPathfinder aStarPathfinder;

    public Vector2Int pathStart;
    public Vector2Int pathGoal;
    public bool randomPath;

    #region Ressource

    private RessourceAct _ressourceAct;
    [SerializeField] private int _ressourceNb;
    [SerializeField] private string _ressourceMin;

    #endregion

    #region Ennemi

    [SerializeField] private Transform _ennemiParent;
    [SerializeField] private float detectRange;

    #endregion

    #region Construction

    [SerializeField] private Transform _constructionParent;
    private List<Construction> _constructionInProgresse = new List<Construction>();
    private GameObject _nearestConstruction;

    #endregion


    void Start()
    {
        _ressourceAct = RessourceAct.Instance;
    }

    void Update()
    {
        print(CheckRessource());
        if (CheckEnnemi()) //check if ennemy near
        {
            print("Ennemi");
        }
        else if (CheckConstrucionInProgresse()) //Check si construction a faire
        {
            print("go construction");
            CheckNearestConstruction();
            TaskManager.Instance.AssignTaskToAgent(agent, "Build Camp");
        }
        else if (CheckRessource()) //check ressource
        {
            print("go To ressource");
            RecoltRessource();
        }
        else //balade
        {
            print(_ressourceAct.gameObject.transform.childCount);
            print("balade");
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
        agent.target = _nearestConstruction;
    }

    #endregion

    #region Ressource

    private bool CheckRessource()
    {
        if ((_ressourceAct.GetWood() < _ressourceAct.maxRessource ||
             _ressourceAct.GetStone() < _ressourceAct.maxRessource ||
             _ressourceAct.GetFood() < _ressourceAct.maxRessource) &&
            _ressourceAct.gameObject.transform.childCount >= 3)
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

        if (_ressourceNb < _ressourceAct.maxRessource && _ressourceNb > 0)
        {
            ChooseRessource(_ressourceMin);
        }
        else
        {
            print("No ressource to collect");
        }
    }

    void GetLessRessource(Dictionary<string, int> ressource)
    {
        _ressourceNb = 0;
        var minRessource = ressource.OrderBy(_ressource => _ressource.Value).First();
        _ressourceMin = minRessource.Key;
        while (_ressourceNb == 0)
        {
            switch (_ressourceMin)
            {
                case "Wood": //Wood
                    _ressourceNb = GameObject.Find("Wood").transform.childCount;
                    if (_ressourceNb == 0)
                    {
                        _ressourceMin = "Rock";
                    }

                    break;

                case "Rock": //Rock
                    _ressourceNb = GameObject.Find("Rock").transform.childCount;
                    if (_ressourceNb == 0)
                    {
                        _ressourceMin = "Meat";
                    }

                    break;

                case "Meat": //Meat
                    _ressourceNb = GameObject.Find("Meat").transform.childCount;
                    if (_ressourceNb == 0)
                    {
                        _ressourceMin = "Default";
                    }

                    break;

                default:
                    _ressourceNb = -1;
                    break;
            }
        }

        if (_ressourceNb == -1)
        {
            print("No ressource to collect");
        }
    }

    void ChooseRessource(string indexRessource)
    {
        switch (indexRessource)
        {
            case "Wood": //Wood
                TaskManager.Instance.AssignTaskToAgent(agent, "Collect Resources",
                    new Dictionary<string, int> { { "Wood", 10 } });
                break;
            case "Rock": //Rock
                TaskManager.Instance.AssignTaskToAgent(agent, "Collect Resources",
                    new Dictionary<string, int> { { "Rock", 10 } });
                break;
            case "Meat": //Meat
                TaskManager.Instance.AssignTaskToAgent(agent, "Collect Resources",
                    new Dictionary<string, int> { { "Meat", 10 } });
                break;
            default:
                print("Invalid ressource");
                break;
        }
    }

    #endregion
}