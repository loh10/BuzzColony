using System.Collections.Generic;

public class MyTask
{
    public string TaskName { get; set; }

    public int Priority { get; set; }

    // Dictionary of resources required for the task and their quantities
    public Dictionary<string, int> RequiredResources { get; set; }

    // List of task names that are dependencies for this task
    public List<string> Dependencies { get; set; }

    public MyTask(string taskName, int priority, Dictionary<string, int> requiredResources = null, List<string> dependencies = null)
    {
        TaskName = taskName;
        Priority = priority;
        RequiredResources = requiredResources ?? new Dictionary<string, int>();
        Dependencies = dependencies ?? new List<string>();
    }
}
