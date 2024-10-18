using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    public Agent agent;
    public AStarPathfinder AStarPathfinder;

    public Vector2Int pathStart;
    public Vector2Int pathGoal;
    public bool randomPath;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int targetX = randomPath ? Random.Range(0, 200) : pathGoal.x;
            int targetY = randomPath ? Random.Range(0, 200) : pathGoal.y;

            List<Node> path = AStarPathfinder.FindPath(new Vector2Int(100, 100), new Vector2Int(targetX, targetY));
            print(targetX + "" + targetY);
            //TaskManager.Instance.AssignTaskToAgent(agent, "Collect Resources", new Dictionary<string, int> { { "Wood", 10 } });
        }
    }
}
