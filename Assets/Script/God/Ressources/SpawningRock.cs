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
    private int index;

    private void Start()
    {
        if (SaveAndLoad.Instance!=null)
        {
            index = SaveAndLoad.Instance._nbConstruction;
            for (int i = 0; i < index; i++)
            {
                rock = Instantiate(rockPrefab, _spawning.StringToVector2(SaveAndLoad.Instance._rock["Rock" + i]), Quaternion.identity);
                rock.transform.SetParent(rockParent.transform);
                rock.name = "Rock" + i;
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
                rock = Instantiate(rockPrefab, _posToBuild, Quaternion.identity);
                rock.transform.SetParent(rockParent.transform);
                rock.name = "Rock" + index;
                _canBuild = false;
            }
        }
    }
}