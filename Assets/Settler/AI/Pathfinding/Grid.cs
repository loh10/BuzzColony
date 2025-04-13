using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int Width = 10;
    public int Height = 10;
    public GameObject nodePrefab; // Prefab for visualizing each node
    public Node[,] Nodes;

    // Dictionary to store node objects by position for visualization
    private Dictionary<Vector2Int, GameObject> nodeObjects;

    public void InitializeGrid()
    {
        Nodes = new Node[Width, Height];
        nodeObjects = new Dictionary<Vector2Int, GameObject>();

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Vector2Int position = new Vector2Int(x, y);
                bool walkable = true; // Set to false for obstacles

                // Instantiate the Node prefab
                GameObject nodeInstance = Instantiate(nodePrefab, new Vector3(x, y, 0), Quaternion.identity);

                // Initialize the Node component
                Node nodeComponent = nodeInstance.GetComponent<Node>();
                nodeComponent.Initialize(position, walkable);

                // Store the node reference
                Nodes[x, y] = nodeComponent;

                // Store the node object for visualization
                nodeObjects[position] = nodeInstance;

                // Set the initial color based on walkability
                nodeInstance.GetComponent<SpriteRenderer>().color = walkable ? Color.white : Color.red;
            }
        }
    }

    // Get the GameObject associated with a node position
    public GameObject GetNodeObject(Vector2Int position)
    {
        if (nodeObjects.ContainsKey(position))
        {
            return nodeObjects[position];
        }
        return null;
    }

    // Same GetNeighbors and IsPositionInBounds methods as before
    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        // Neighbor directions including diagonals
        Vector2Int[] neighborOffsets = {
        Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right,
        new Vector2Int(1, 1),     // Top-right diagonal
        new Vector2Int(-1, 1),    // Top-left diagonal
        new Vector2Int(1, -1),    // Bottom-right diagonal
        new Vector2Int(-1, -1)    // Bottom-left diagonal
    };

        foreach (var offset in neighborOffsets)
        {
            Vector2Int neighborPos = node.Position + offset;

            if (IsPositionInBounds(neighborPos))
            {
                Node neighborNode = Nodes[neighborPos.x, neighborPos.y];

                // Check for diagonal movement restrictions
                if (offset == new Vector2Int(1, 1) || offset == new Vector2Int(-1, -1)) // Top-right or Bottom-left
                {
                    Vector2Int horizontalNeighbor = new Vector2Int(neighborPos.x - 1, neighborPos.y);
                    Vector2Int verticalNeighbor = new Vector2Int(neighborPos.x, neighborPos.y - 1);

                    // Ensure both adjacent nodes are walkable
                    if (IsPositionInBounds(horizontalNeighbor) && IsPositionInBounds(verticalNeighbor))
                    {
                        if (Nodes[horizontalNeighbor.x, horizontalNeighbor.y].IsWalkable &&
                            Nodes[verticalNeighbor.x, verticalNeighbor.y].IsWalkable)
                        {
                            neighbors.Add(neighborNode);
                        }
                    }
                }
                else if (offset == new Vector2Int(1, -1) || offset == new Vector2Int(-1, 1)) // Bottom-right or Top-left
                {
                    Vector2Int horizontalNeighbor = new Vector2Int(neighborPos.x - 1, neighborPos.y);
                    Vector2Int verticalNeighbor = new Vector2Int(neighborPos.x, neighborPos.y + 1);

                    // Ensure both adjacent nodes are walkable
                    if (IsPositionInBounds(horizontalNeighbor) && IsPositionInBounds(verticalNeighbor))
                    {
                        if (Nodes[horizontalNeighbor.x, horizontalNeighbor.y].IsWalkable &&
                            Nodes[verticalNeighbor.x, verticalNeighbor.y].IsWalkable)
                        {
                            neighbors.Add(neighborNode);
                        }
                    }
                }
                else
                {
                    // For cardinal neighbors, simply check if they're walkable
                    if (neighborNode.IsWalkable)
                    {
                        neighbors.Add(neighborNode);
                    }
                }
            }
        }

        return neighbors;
    }



    private bool IsPositionInBounds(Vector2Int position)
    {
        return position.x >= 0 && position.x < Width && position.y >= 0 && position.y < Height;
    }
}
