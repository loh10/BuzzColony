using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dodo.SerializedCollections
{
    public class Agent : MonoBehaviour
    {
        public List<MyTask> TaskList = new List<MyTask>();

        private TaskManager taskManager;

        // Simulated resource available for tasks
        private int woodAvailable = 0;

        private void Start()
        {
            // Find the TaskManager in the scene
            taskManager = FindObjectOfType<TaskManager>();

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
            // Example: Check if there's enough wood to build
            if (task.TaskName == "Build Camp" && woodAvailable < 5)
            {
                Debug.Log("Not enough wood to build. Need to collect wood.");
                return false;
            }

            // Check each dependency and make sure they are fulfilled
            foreach (string dependency in task.Dependencies)
            {
                if (dependency == "Collect Wood" && woodAvailable < 5)
                {
                    return false;
                }
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
                    // Check if the task's dependencies are fulfilled
                    if (AreDependenciesMet(currentTask))
                    {
                        Debug.Log("Performing task: " + currentTask.TaskName);

                        // Execute the task
                        yield return StartCoroutine(ExecuteTask(currentTask.TaskName, 2.0f));

                        // Example: If the task is to collect wood, increment available wood
                        if (currentTask.TaskName == "Collect Wood")
                        {
                            woodAvailable += 5;
                            Debug.Log("Collected wood. Wood available: " + woodAvailable);
                        }

                        // Remove completed task from the list
                        TaskList.Remove(currentTask);
                    }
                    else
                    {
                        // If dependencies are not met, add the dependent task
                        foreach (string dependency in currentTask.Dependencies)
                        {
                            if (dependency == "Collect Wood")
                            {
                                //taskManager.AssignTaskToAgent(this, "Collect Wood");
                            }
                        }
                    }
                }

                yield return null;
            }
        }

        // Simulates the execution of a task
        IEnumerator ExecuteTask(string taskName, float duration)
        {
            // Simulate task duration
            yield return new WaitForSeconds(duration);
        }
    }
}


