using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawningWood : MonoBehaviour
{
    public GameObject woodPrefab;
    public GameObject woodParent;
    private GameObject wood;
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
                wood = Instantiate(woodPrefab, _posToBuild, Quaternion.identity);  
                wood.transform.SetParent(woodParent.transform);
                _canBuild = false;
            }
        }
    }
}