using Dodo.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    public Agent agent;
    //public AStarPathfinder AStarPathfinder;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //print("HEY");
            //int targetX = Random.Range(1, 9);
            //int targetY = Random.Range(1, 9);
            //List<Node> path = AStarPathfinder.FindPath(new Vector2Int(0, 0), new Vector2Int(targetX, targetY));
            TaskManager.Instance.AssignTaskToAgent(agent, "Collect Resources", new Dictionary<string, int> { { "Wood", 10 } });

        }
    }
}
