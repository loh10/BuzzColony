using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawningRock : MonoBehaviour
{
    public GameObject rockPrefab;
    public GameObject rockParent;
    private GameObject rock;
    public Tilemap tilemap;
    private Spawning _spawning = new Spawning();
    private bool _canBuild;
    private Vector3 _posToBuild;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _posToBuild = _spawning.SpawnPosition(tilemap, out _canBuild);
            if (_canBuild)
            {
                rock = Instantiate(rockPrefab, _posToBuild, Quaternion.identity);
                rock.transform.SetParent(rockParent.transform);
                _canBuild = false;
            }
        }
    }
}