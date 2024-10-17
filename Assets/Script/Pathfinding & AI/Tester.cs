using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    public Agent agent;
    public AStarPathfinder AStarPathfinder;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int targetX = Random.Range(1, 9);
            int targetY = Random.Range(1, 9);
            //List<Node> path = AStarPathfinder.FindPath(new Vector2Int(0, 0), new Vector2Int(0, 158));
            List<Node> path = AStarPathfinder.FindPath(new Vector2Int(0, 0), new Vector2Int(Random.Range(1, 200), Random.Range(1, 200)));
            print(targetX + "" + targetY);
            //TaskManager.Instance.AssignTaskToAgent(agent, "Collect Resources", new Dictionary<string, int> { { "Wood", 10 } });

        }
    }
}
