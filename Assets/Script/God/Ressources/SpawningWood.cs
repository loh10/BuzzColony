using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawningWood : MonoBehaviour
{
    public GameObject woodPrefab;
    public GameObject woodParent;
    private GameObject wood;
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
                wood = Instantiate(woodPrefab, _spawning.StringToVector2(SaveAndLoad.Instance._wood["Wood" + i]), Quaternion.identity);
                wood.transform.SetParent(woodParent.transform);
                wood.name = "Wood" + i;
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
                wood = Instantiate(woodPrefab, _posToBuild, Quaternion.identity);
                wood.transform.SetParent(woodParent.transform);
                wood.name = "Wood" + index;
                _canBuild = false;
            }
        }
    }
}