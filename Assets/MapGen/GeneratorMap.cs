using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GeneratorMap : MonoBehaviour
{
    #region MapDetail
    public Node[,] Nodes;
    public GameObject nodePrefab;
    public Transform nodeParent;
    private Dictionary<Vector2Int, GameObject> nodeObjects;
    [Header("Mesh Options")] public bool generate;
    public int mapScale = 25;

    #endregion

    #region MapNoiseOption

    [Header("Map Noise Options")] [Range(0, 1)]
    public float mapPerlinScale = 0.5f;

    [Range(0, 1)] public float mapPersistence = .5f;
    [Range(0.001f, 1)] float mapLacunarity = 0.05f;
    public int mapOctaveCount = 1;

    [Header("Mountain Noise Options")] [Range(0, 1)]
    public float mountainPerlinScale = 0.5f;

    [Range(0, 1)] public float mountainPersistence = .5f;
    [Range(0.001f, 1)] float mountainLacunarity = 0.05f;
    public float mountainRatio = .75f;
    public int mountainOctaveCount = 1;
    public int seed;

    #endregion

    #region NoiseOption

    [Header("Tile Options")] private float _colorForce;
    public List<Tile> tileMap;
    public Tile tileMap2;
    public Tilemap tileMapLayer1, tileMapLayer2;

    #endregion

    void Start()
    {
        ClearTileMaps();
        if (SaveAndLoad.Instance != null)
        {
            SaveAndLoad.Instance.LoadGame();
            seed = SaveAndLoad.Instance._seed;
        }

        GenerateMap();
    }

    void ClearTileMaps()
    {
        tileMapLayer1.ClearAllTiles();
        tileMapLayer2.ClearAllTiles();
    }

    /// <summary>
    /// genere la map sur mapSize par mapSize
    /// </summary>
    void GenerateMap()
    {
        Nodes = new Node[mapScale, mapScale];
        nodeObjects = new Dictionary<Vector2Int, GameObject>();

        for (int i = 0; i < mapScale; i++)
        {
            for (int j = 0; j < mapScale; j++)
            {
                SetColor(i, j);
                SetMountain(i, j);
            }
        }
    }

    /// <summary>
    /// Set la Tile sur la map selon la couleur dans perlin
    /// </summary>
    void SetColor(int x, int z)
    {
        float colorFloat = CalculatePerlin(x, z, mapPerlinScale, mapOctaveCount, mapLacunarity, mapPersistence);
        int indexColor;
        switch (colorFloat)
        {
            case < 0.7f:
                indexColor = 0;
                break;
            case < 0.8f:
                indexColor = 1;
                break;
            case < 0.9f:
                indexColor = 2;
                break;
            default:
                indexColor = 3;
                break;
        }

        tileMapLayer1.SetTile(new Vector3Int(x, z, 0), tileMap[indexColor]);
    }

    void SetMountain(int x, int z)
    {
        float colorMountain = CalculatePerlin(x, z, mountainPerlinScale, mountainOctaveCount, mountainLacunarity,
            mountainPersistence);
        if (colorMountain > mountainRatio)
        {
            tileMapLayer2.SetTile(new Vector3Int(x, z, 0), tileMap2);
            if (tileMapLayer1.GetTile(new Vector3Int(x, z, 0)))
            {
                tileMapLayer1.SetTile(new Vector3Int(x, z, 0), null);
                // Instantiate(nodePrefab, new Vector3(x, z, 0), Quaternion.identity);
                SetNode(x, z, false);
            }
        }
        else
        {
            // Instantiate(nodePrefab, new Vector3(x, z, 0), Quaternion.identity);
            SetNode(x, z, true);
        }
    }

    void SetNode(int x, int z, bool walkable)
    {
        Vector2Int position = new Vector2Int(x, z);

        // Instantiate the Node prefab
        GameObject nodeInstance = Instantiate(nodePrefab, new Vector3(x, z, 0), Quaternion.identity);

        // Initialize the Node component
        Node nodeComponent = nodeInstance.GetComponent<Node>();
        nodeComponent.Initialize(position, walkable);
        Nodes[x, z] = nodeComponent;

        // Store the node object for visualization
        nodeObjects[position] = nodeInstance;

        // Set the initial color based on walkability
        nodeInstance.GetComponent<SpriteRenderer>().color = walkable ? Color.white : Color.red;
        nodeInstance.transform.SetParent(nodeParent);
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
        return position.x >= 0 && position.x < mapScale && position.y >= 0 && position.y < mapScale;
    }

    /// <summary>
    /// Retourne un float via le perlin Noise
    /// </summary>
    float CalculatePerlin(int x, int z, float scale, int octave, float lacunarity, float persistency)
    {
        float number = 0;
        float amplitude = 1;
        float frequence = scale;

        for (int i = 0; i < octave; i++)
        {
            number += Mathf.PerlinNoise(x * frequence + seed, z * frequence + seed) * amplitude;
            frequence /= lacunarity;
            amplitude *= persistency;
        }

        return number;
    }
}