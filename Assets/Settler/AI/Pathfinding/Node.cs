using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField]
    private Vector2Int position;

    [SerializeField] 
    private bool isWalkable;

    [SerializeField] 
    private int gCost;

    [SerializeField] 
    private int hCost;

    public Node Parent { get; set; }

    // Properties to get values without setters
    
    public Vector2Int Position => position; // Only a getter to expose position
    public bool IsWalkable => isWalkable; // Only a getter to expose IsWalkable
    public int GCost
    {
        get => gCost;
        set => gCost = value;
    }

    public int HCost
    {
        get => hCost;
        set => hCost = value;
    }

    // Property to get the FCost
    public int FCost => GCost + HCost;

    // Constructor moved to Initialize method
    public void Initialize(Vector2Int position, bool isWalkable)
    {
        this.position = position;
        this.isWalkable = isWalkable;
    }
}
