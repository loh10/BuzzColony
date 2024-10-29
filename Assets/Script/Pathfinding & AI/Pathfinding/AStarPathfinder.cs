using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinder : MonoBehaviour
{
    private GeneratorMap grid;

    public Agent agent;

    private void Start()
    {
        grid = FindObjectOfType<GeneratorMap>();
        agent = FindObjectOfType<Agent>();
    }

    public List<Node> FindPath(Vector2Int start, Vector2Int goal)
    {
        Node startNode = grid.Nodes[start.x, start.y];
        Node goalNode = grid.Nodes[goal.x, goal.y];

        if(!goalNode.IsWalkable)
        {
            Debug.LogWarning("Goal Node is not walkable");
        }

        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].FCost < currentNode.FCost || (openList[i].FCost == currentNode.FCost && openList[i].HCost < currentNode.HCost))
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == goalNode)
            {
                // Path found, retrace it and assign to the agent
                List<Node> path = RetracePath(startNode, goalNode);
                if(agent)
                {

                    agent.SetPath(path);
                }
                else
                {
                    Debug.LogWarning("Failed to find a agent for the path");
                }
                return path;
            }

            foreach (Node neighbor in grid.GetNeighbors(currentNode))
            {
                if (!neighbor.IsWalkable || closedList.Contains(neighbor))
                {
                    continue;
                }

                int newGCost = currentNode.GCost + GetDistance(currentNode, neighbor);
                if (newGCost < neighbor.GCost || !openList.Contains(neighbor))
                {
                    neighbor.GCost = newGCost;
                    neighbor.HCost = GetDistance(neighbor, goalNode);
                    neighbor.Parent = currentNode;

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }
        }

        return null;
    }

    private List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        path.Reverse();
        VisualizePath(path);
        return path;
    }

    private void VisualizePath(List<Node> path)
    {
        foreach (Node node in path)
        {
            // Get the GameObject of the node from the grid
            GameObject nodeObject = grid.GetNodeObject(node.Position);
            if (nodeObject != null)
            {
                // Change the color of the node to green to represent the path
                nodeObject.GetComponentInChildren<SpriteRenderer>().color = Color.green;
            }
        }
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.Position.x - nodeB.Position.x);
        int dstY = Mathf.Abs(nodeA.Position.y - nodeB.Position.y);

        // If moving diagonally, return the diagonal distance
        return Mathf.Max(dstX, dstY);
    }

}
