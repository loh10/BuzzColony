using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Agent : MonoBehaviour
{
    [SerializeField] public List<MyTask> TaskList = new List<MyTask>();

    // Simulated resource available for tasks
    private Dictionary<string, int> resourcesAvailable = new Dictionary<string, int>();
    public bool isWorking;
    public GameObject target;
    public float speed;

    private List<Node> currentPath; // The path to follow
    private int currentNodeIndex = 0;
    private AStarPathfinder starPathfinder;

    public enum agentTask
    {
        collectWood,
        fightEnemy,
    }

    public agentTask[] tasks;

    private void Start()
    {
        starPathfinder = GameObject.Find("Pathfinder").GetComponent<AStarPathfinder>();
        // Initialize resources (e.g., wood and stone)
        resourcesAvailable["Wood"] = 0;
        resourcesAvailable["Stone"] = 0;

        // Start the task execution routine
        //StartCoroutine(PerformTaskRoutine());
    }

    // Adds a task to the agent's task list
    public void AddTask(MyTask task)
    {
        TaskList.Add(task);
        if (task.TaskName == "Collect Resources")
        {
            Debug.Log(this.gameObject.name + " is going to " + task.TaskName);
        }

        StartCoroutine(PerformTaskRoutine());
    }

    // Retrieves the next task based on priority
    public MyTask GetNextTask()
    {
        return TaskList.Count > 0 ? TaskList[0] : null;
    }

    public bool AreDependenciesMet(MyTask task)
    {
        foreach (var resourceRequirement in task.DynamicParameters)
        {
            string resourceName = resourceRequirement.Key;
            int requiredAmount = resourceRequirement.Value;

            // If we don't have enough of a required resource, return false
            //if (resourcesAvailable.ContainsKey(resourceName) && resourcesAvailable[resourceName] < requiredAmount)
            //{
            //    Debug.Log($"Not enough {resourceName}. Need to collect {requiredAmount - resourcesAvailable[resourceName]} more.");
            //    return false;
            //}
        }

        return true;
    }

    // Coroutine to handle task execution in a loop
    IEnumerator PerformTaskRoutine()
    {
        while (true)
        {
            MyTask currentTask = GetNextTask();

            if (currentTask != null)
            {
                //print("Task name : " + currentTask.TaskName + "  Task priority : " + currentTask.Priority);
                print(TaskList[0].TaskName);
                if (AreDependenciesMet(currentTask))
                {
                    isWorking = true;

                    yield return StartCoroutine(ExecuteTask(currentTask, 2f));

                    // Handle specific task results (like resource collection)
                    if (currentTask.TaskName == "Collect Resources")
                    {
                        foreach (var resourceRequirement in currentTask.DynamicParameters)
                        {
                            string resourceName = resourceRequirement.Key;
                            int collectedAmount = resourceRequirement.Value;
                            // Add the collected resource to the available resources

                            switch (resourceName)
                            {
                                case "Wood":
                                    target = GetClosestRessource(GameObject.FindGameObjectsWithTag("Wood"));
                                    break;
                                case "Meat":
                                    target = GetClosestRessource(GameObject.FindGameObjectsWithTag("Meat"));
                                    break;
                                case "Rock":
                                    target = GetClosestRessource(GameObject.FindGameObjectsWithTag("Rock"));
                                    break;
                                default:
                                    target = new GameObject();
                                    print("nothing to collect");
                                    break;
                            }

                            int targetX = (int)target.transform.position.x;
                            int targetY = (int)target.transform.position.y;
                            List<Node> path = starPathfinder.FindPath(
                                new Vector2Int((int)transform.position.x, (int)transform.position.y),
                                new Vector2Int(targetX, targetY), this);

                            if (resourcesAvailable.ContainsKey(resourceName))
                            {
                                resourcesAvailable[resourceName] += collectedAmount;
                            }
                            else
                            {
                                resourcesAvailable[resourceName] = collectedAmount;
                            }
                            //Test
                            //Debug.Log($"Collected {collectedAmount} units of {resourceName}. Available: {resourcesAvailable[resourceName]}");
                        }
                    }

                    TaskList.Remove(currentTask);
                }
                else
                {
                    Debug.Log($"Cannot perform task '{currentTask.TaskName}' yet. Dependencies not met.");
                }
            }

            yield return null;
        }
    }

    private GameObject GetClosestRessource(GameObject[] ressource)
    {
        GameObject nearestProduct;
        nearestProduct = ressource[0];
        foreach (GameObject res in ressource)
        {
            if (Vector3.Distance(this.transform.position, res.transform.position) <
                Vector3.Distance(this.transform.position, nearestProduct.transform.position))
            {
                nearestProduct = res;
            }
        }

        return nearestProduct;
    }

    IEnumerator ExecuteTask(MyTask task, float duration)
    {
        yield return new WaitForSeconds(duration);
    }

    private void Update()
    {
        // If the agent is currently moving along a path, move towards the next node
        if (isWorking && currentPath != null && currentNodeIndex < currentPath.Count+1)
        {
            MoveAlongPath();
        }
    }

    // Start moving along the path
    public void SetPath(List<Node> path)
    {
        if (path == null || path.Count == 0)
        {
            Debug.LogWarning("Path is invalid or empty.");
            return;
        }

        currentPath = path;
        currentNodeIndex = 0;
        isWorking = true;
    }

    // Move the agent towards the next node in the path
    private void MoveAlongPath()
    {
        if (currentNodeIndex >= currentPath.Count)
        {
            // Finished the path
            Collider2D[] collision = Physics2D.OverlapCircleAll(transform.position, 0.6f);
            foreach (var col in collision)
            {
                if (col.gameObject.tag == "Wood")
                {
                    Destroy(col.gameObject);
                    RessourceAct.Instance.AddRessource(1,Ressource.Bois);
                }
                else if (col.gameObject.tag == "Rock")
                {
                    Destroy(col.gameObject);
                    RessourceAct.Instance.AddRessource(1,Ressource.Roche);
                }
                else if (col.gameObject.tag == "Meat")
                {
                    Destroy(col.gameObject);
                    RessourceAct.Instance.AddRessource(1,Ressource.Nourriture);
                }
            }
            isWorking = false;
            return;
        }

        Node targetNode = currentPath[currentNodeIndex];
        Vector3 targetPosition = new Vector3(targetNode.Position.x, targetNode.Position.y, transform.position.z);

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentNodeIndex++;
        }
    }
}