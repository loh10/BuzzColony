using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isAgressive = false;

    public float detectionRange = 5f;
    public float attackRange = 1f;
    public float attackCooldown = 1.5f;
    public int attackDamage = 10;
    private float lastAttackTime;

    private Transform targetColon;
    private Agent pathfindingAgent; 
    private List<Node> currentPath; 
    private int currentNodeIndex = 0;

    private void Start()
    {
        pathfindingAgent = GetComponent<Agent>();
    }

    private void Update()
    {
        if (!isAgressive) return;

        DetectColons();

        if (targetColon != null)
        {
            float distanceToColon = Vector2.Distance(transform.position, targetColon.position);

            if (distanceToColon <= attackRange && Time.time >= lastAttackTime + attackCooldown)
            {
                AttackColon();
                lastAttackTime = Time.time;
            }
            else if (currentPath == null || distanceToColon > attackRange)
            {
                CalculatePathToColon();
            }

            if (currentPath != null && currentNodeIndex < currentPath.Count)
            {
                MoveAlongPath();
            }
        }
    }

    private void DetectColons()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(transform.position, detectionRange);
        foreach (Collider2D col in detectedObjects)
        {
            if (col.CompareTag("Colon"))
            {
                targetColon = col.transform;
                break;
            }
        }
    }

    private void CalculatePathToColon()
    {
        if (pathfindingAgent != null && targetColon != null)
        {
            int targetX = (int)targetColon.position.x;
            int targetY = (int)targetColon.position.y;
            Vector2Int startPos = new Vector2Int((int)transform.position.x, (int)transform.position.y);
            Vector2Int targetPos = new Vector2Int(targetX, targetY);

            currentPath = pathfindingAgent.starPathfinder.FindPath(startPos, targetPos);
            currentNodeIndex = 0;
        }
    }

    private void MoveAlongPath()
    {
        if (currentNodeIndex >= currentPath.Count) return;

        Node targetNode = currentPath[currentNodeIndex];
        Vector3 targetPosition = new Vector3(targetNode.Position.x, targetNode.Position.y, transform.position.z);

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, pathfindingAgent.speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentNodeIndex++;
        }
    }

    private void AttackColon()
    {
        Debug.Log("Attacking colon!");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
