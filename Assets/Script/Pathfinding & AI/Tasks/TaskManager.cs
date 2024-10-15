using System.Collections.Generic;
using UnityEngine;

namespace AYellowpaper.SerializedCollections
{
    public class TaskManager : MonoBehaviour
    {
        public static TaskManager Instance;

        // Dictionary holding all tasks available in the game
        [SerializeField]
        public List<string> Tasks = new List<string>();
        public Dictionary<string, MyTask> AllTasks = new Dictionary<string, MyTask>();
        private ResourceManager resourceManager;

        private void Awake()
        {
            //DontDestroyOnLoad(gameObject);
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            // Find the ResourceManager in the scene
            resourceManager = ResourceManager.Instance;

            // Initialize all possible tasks
            InitializeTasks();
        }

        private void InitializeTasks()
        {
            // Example tasks with resource requirements and optional dependencies
            Dictionary<string, int> collectResources = new Dictionary<string, int> { { "Wood", 5 }, { "Stone", 3 } };
            AllTasks.Add("Collect Resources", new MyTask("Collect Resources", 1, collectResources));
            AllTasks.Add("Build Camp", new MyTask("Build Camp", 2, null, new List<string> { "Collect Resources" }));
            AllTasks.Add("Eat Food", new MyTask("Eat Food", 0));

            Tasks.Add("Collect Resources");
            Tasks.Add("Build Camp");
            Tasks.Add("Eat Food");
        }

        // Retrieve a task by name
        public MyTask GetTaskByName(string taskName)
        {
            if (AllTasks.ContainsKey(taskName))
            {
                return AllTasks[taskName];
            }
            else
            {
                Debug.LogWarning($"Task '{taskName}' not found.");
                return null;
            }
        }

        // Assign a task to an agent
        public void AssignTaskToAgent(Agent agent, string taskName)
        {
            MyTask task = GetTaskByName(taskName);
            if (task != null)
            {
                agent.AddTask(task);
            }
        }
    }

}

