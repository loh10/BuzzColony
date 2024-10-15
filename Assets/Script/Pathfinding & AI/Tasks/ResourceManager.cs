using System.Collections.Generic;
using UnityEngine;

namespace AYellowpaper.SerializedCollections
{
    public class ResourceManager : MonoBehaviour
    {
        public static ResourceManager Instance;

        // Dictionary to hold different resources and their quantities
        [SerializedDictionary]
        public SerializedDictionary<string, int> resources = new SerializedDictionary<string, int>();

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
            Debug.Log($"{amount} {resourceName} added. Current {resourceName}: {resources[resourceName]}");
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

        // Checks if a specific amount of a resource is available
        public bool IsResourceAvailable(string resourceName, int amount)
        {
            return resources.ContainsKey(resourceName) && resources[resourceName] >= amount;
        }

        // Get current amount of a specific resource
        public int GetResourceAmount(string resourceName)
        {
            return resources.ContainsKey(resourceName) ? resources[resourceName] : 0;
        }
    }
}

