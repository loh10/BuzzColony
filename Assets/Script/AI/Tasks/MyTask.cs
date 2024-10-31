using System.Collections.Generic;

public class MyTask
{
    public string TaskName { get; private set; }
    public int Priority { get; private set; }
    public Dictionary<string, int> ResourceRequirements { get; private set; }
    public List<string> Dependencies { get; private set; }

    // Additional parameters for dynamic values
    public Dictionary<string, int> DynamicParameters { get; private set; }

    public MyTask(string taskName, int priority, Dictionary<string, int> resourceRequirements = null, List<string> dependencies = null)
    {
        TaskName = taskName;
        Priority = priority;
        ResourceRequirements = resourceRequirements ?? new Dictionary<string, int>();
        Dependencies = dependencies ?? new List<string>();

        // Initialize dynamic parameters (can be filled in later)
        DynamicParameters = new Dictionary<string, int>();
    }

    // Set dynamic parameters like resource quantity
    public void SetDynamicParameters(Dictionary<string, int> parameters)
    {
        DynamicParameters = parameters;
    }
}
