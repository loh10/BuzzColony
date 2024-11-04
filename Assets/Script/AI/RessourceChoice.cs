using System.Collections.Generic;
using System.Linq;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class RessourceChoice : MonoBehaviour
{
    public Agent agent;
    public AStarPathfinder aStarPathfinder;

    public Vector2Int pathStart;
    public Vector2Int pathGoal;
    public bool randomPath;
    private RessourceAct _ressourceAct;
    [SerializeField]private int _ressourceNb;
    [SerializeField]private string _ressourceMin;

    void Start()
    {
        _ressourceAct = RessourceAct.Instance;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RecoltRessource();
        }
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
        if(_ressourceNb == -1)
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
}