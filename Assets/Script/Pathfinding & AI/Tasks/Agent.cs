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
    public GameObject tree;
    public float speed;

    private void Start()
    {

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

    private void Update()
    {
        if (isWorking)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, tree.transform.position, step);
        }
    }
}

