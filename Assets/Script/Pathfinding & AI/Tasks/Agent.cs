using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Agent : MonoBehaviour
{
    [SerializeField]
    public List<MyTask> TaskList = new List<MyTask>();

    // Simulated resource available for tasks
    private Dictionary<string, int> resourcesAvailable = new Dictionary<string, int>();
    public bool isWorking;
    public GameObject target;
    public float speed;

    private List<Node> currentPath;  // The path to follow
    private int currentNodeIndex = 0;
    private AStarPathfinder starPathfinder;

    private void Start()
    {
        starPathfinder = GameObject.Find("Pathfinder").GetComponent<AStarPathfinder>();
        // Initialize resources (e.g., wood and stone)
        resourcesAvailable["Wood"] = 0;
        resourcesAvailable["Stone"] = 0;

        // Start the task execution routine
        StartCoroutine(PerformTaskRoutine());
    }

    // Adds a task to the agent's task list
    public void AddTask(MyTask task)
    {
        TaskList.Add(task);
        Debug.Log(task.TaskName);
        if(task.TaskName == "Collect Resources")
        {

        }
    }

    // Retrieves the next task based on priority
    public MyTask GetNextTask()
    {
        // Sort the task list by priority and return the highest priority task
        return TaskList.Count > 0 ? TaskList[0] : null;
    }

    // Checks if the task's dependencies are met before execution
    public bool AreDependenciesMet(MyTask task)
    {
        // Iterate through the dynamic parameters to see if resource requirements are met
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
                if (AreDependenciesMet(currentTask))
                {
                    isWorking = true;
                    Debug.Log("Performing task: " + currentTask.TaskName);

                    // Execute the task
                    yield return StartCoroutine(ExecuteTask(currentTask));

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
                                    target = GameObject.Find("Tree temp");
                                    break;
                            }
                            int targetX = (int)target.transform.position.x;
                            int targetY = (int)target.transform.position.y;
                            List<Node> path = starPathfinder.FindPath(new Vector2Int((int)transform.position.x, (int)transform.position.y), new Vector2Int(targetX, targetY));

                            if (resourcesAvailable.ContainsKey(resourceName))
                            {
                                resourcesAvailable[resourceName] += collectedAmount;
                            }
                            else
                            {
                                resourcesAvailable[resourceName] = collectedAmount;
                            }
                            //Test
                            Debug.Log($"Collected {collectedAmount} units of {resourceName}. Available: {resourcesAvailable[resourceName]}");
                        }
                    }

                    // Remove completed task from the list
                    TaskList.Remove(currentTask);
                }
                else
                {
                    // Handle unmet dependencies (you could assign new tasks here)
                    Debug.Log($"Cannot perform task '{currentTask.TaskName}' yet. Dependencies not met.");
                }
            }

            yield return null;
        }
    }

    // Simulates the execution of a task
    IEnumerator ExecuteTask(MyTask task, float duration)
    {
        yield return new WaitForSeconds(duration);
    }

    IEnumerator ExecuteTask(MyTask task)
    {
        yield return new WaitForSeconds(2.0f); ; // default value
    }

    //private void Update()
    //{
    //    if (isWorking)
    //    {
    //        float step = speed * Time.deltaTime;
    //        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
    //    }
    //}

    ///////////////////////////////////////////////////////////////////////////////////////////
    ///
    private void Update()
    {
        // If the agent is currently moving along a path, move towards the next node
        if (isWorking && currentPath != null && currentNodeIndex < currentPath.Count)
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
            isWorking = false;
            return;
        }

        Node targetNode = currentPath[currentNodeIndex];
        Vector3 targetPosition = new Vector3(targetNode.Position.x, targetNode.Position.y, transform.position.z);

        // Move the agent towards the target node
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Check if the agent reached the target node, move to the next node
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentNodeIndex++;
        }
    }
}

