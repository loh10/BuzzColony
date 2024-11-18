using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Agent : MonoBehaviour
{
    public GameObject target;
    public float speed;

    private List<Node> currentPath; // The path to follow
    private int currentNodeIndex = 0;
    private AStarPathfinder starPathfinder;
    private ChoiceState choiceState;

    private void Start()
    {
        starPathfinder = GetComponent<AStarPathfinder>();
        choiceState = GetComponent<ChoiceState>();
    }
    

    private void Update()
    {
        // If the agent is currently moving along a path, move towards the next node
        if (GetComponent<ChoiceState>().isWorking && currentPath != null && currentNodeIndex < currentPath.Count)
        {
            MoveAlongPath();
        }
    }

    // Start moving along the path
    public void SetPath(List<Node> path)
    {
        if (path == null || path.Count == 0)
        {
            if(choiceState.actTask == ActTask.Collect)
             {
                 CheckRessourceNear();
                 choiceState.isWorking = false;
             }
            return;
        }

        currentPath = path;
        currentNodeIndex = 0;
    }

    private void CheckRessourceNear()
    {
        Collider2D[] collision = Physics2D.OverlapCircleAll(transform.position, 0.6f);
        foreach (var col in collision)
        {
            if (col.gameObject.tag == "Wood")
            {
                Destroy(col.gameObject);
                RessourceAct.Instance.AddRessource(1,Ressource.Bois);
                choiceState.actTask = ActTask.Nothing;
                choiceState.isWorking = false;
                return;
            }
            else if (col.gameObject.tag == "Rock")
            {
                Destroy(col.gameObject);
                RessourceAct.Instance.AddRessource(1,Ressource.Roche);
                choiceState.actTask = ActTask.Nothing;
                choiceState.isWorking = false;
                return;
            }
            else if (col.gameObject.tag == "Meat")
            {
                Destroy(col.gameObject);
                RessourceAct.Instance.AddRessource(1,Ressource.Nourriture);
                choiceState.actTask = ActTask.Nothing;
                choiceState.isWorking = false;
                return;
            }
            choiceState.actTask = ActTask.Nothing;
            choiceState.isWorking = false;
        }
    }
    // Move the agent towards the next node in the path
    private void MoveAlongPath()
    {
        CheckRessourceNear();
        Node targetNode = currentPath[currentNodeIndex];
        Vector3 targetPosition = new Vector3(targetNode.Position.x, targetNode.Position.y, transform.position.z);

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentNodeIndex++;
        }
    }
}