using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawningMeat : MonoBehaviour
{
    public GameObject meatPrefab;
    public GameObject meatParent;
    private GameObject meat;
    public Tilemap tilemap;
    private Spawning _spawning = new Spawning();
    private bool _canBuild ;
    private Vector3 _posToBuild;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _posToBuild = _spawning.SpawnPosition(tilemap, out _canBuild);
            if (_canBuild)
            {
                meat = Instantiate(meatPrefab, _posToBuild, Quaternion.identity);  
                meat.transform.SetParent(meatParent.transform);
                _canBuild = false;
            }
        }
    }
}