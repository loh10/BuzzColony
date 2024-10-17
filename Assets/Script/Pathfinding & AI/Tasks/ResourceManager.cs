using System.Collections.Generic;
using UnityEngine;


public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    // Dictionary to hold different resources and their quantities
    // [SerializedDictionary]
    public Dictionary<string, int> resources = new Dictionary<string, int>();

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
        //Initialize dictionary values
        AddResource("Wood", 0);
        AddResource("Stone", 0);
        AddResource("Meat", 0);
    }

    public void AddResource(string resourceName, int amount)
    {
        if (resources.ContainsKey(resourceName))
        {
            resources[resourceName] += amount;
        }
        else
        {
            resources[resourceName] = amount;
        }
        //Debug.Log($"{amount} {resourceName} added. Current {resourceName}: {resources[resourceName]}");
    }

    // Removes a resource from the resource pool if available
    public bool UseResource(string resourceName, int amount)
    {
        if (resources.ContainsKey(resourceName) && resources[resourceName] >= amount)
        {
            resources[resourceName] -= amount;
            Debug.Log($"{amount} {resourceName} used. Remaining {resourceName}: {resources[resourceName]}");
            return true;
        }
        else
        {
            Debug.Log($"Not enough {resourceName} available.");
            return false;
        }
    }

    public bool IsResourceAvailable(string resourceName, int amount)
    {
        return resources.ContainsKey(resourceName) && resources[resourceName] >= amount;
    }

    public int GetResourceAmount(string resourceName)
    {
        return resources.ContainsKey(resourceName) ? resources[resourceName] : 0;
    }
}


