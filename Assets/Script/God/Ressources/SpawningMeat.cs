using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawningMeat : MonoBehaviour
{
    public GameObject meatPrefab;
    public GameObject meatParent;
    private GameObject meat;
    public Tilemap tilemap;
    private Spawning _spawning = new Spawning();
    private bool _canBuild;
    private Vector3 _posToBuild;
    private int index;

    private void Start()
    {
        if (SaveAndLoad.Instance!=null)
        {
            index = SaveAndLoad.Instance._nbConstruction;
            for (int i = 0; i < index; i++)
            {
                meat = Instantiate(meatPrefab, _spawning.StringToVector2(SaveAndLoad.Instance._meat["Meat" + i]), Quaternion.identity);
                meat.transform.SetParent(meatParent.transform);
                meat.name = "Meat" + i;
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _posToBuild = _spawning.SpawnPosition(tilemap, out _canBuild);
            if (_canBuild)
            {
                meat = Instantiate(meatPrefab, _posToBuild, Quaternion.identity);
                meat.transform.SetParent(meatParent.transform);
                meat.name = "Meat" + index;
                _canBuild = false;
            }
        }
    }
}