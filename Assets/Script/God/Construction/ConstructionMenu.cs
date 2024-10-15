using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ConstructionMenu : MonoBehaviour
{
    private bool isActive;
    [SerializeField] private GameObject _marketMenu,_currentConstruction;
    [SerializeField] private SpawningMenu _spawningMenu;
    [SerializeField] private Transform _constructionParent;
    public Tilemap tilemap;
    private Vector3 constructionPosition;
    private bool canBuild;
    [CanBeNull] private new Camera camera ;
    private Vector3 worldPoint;
    private Vector3Int cellPosition;
    private Vector3 cellCenterPosition;
    private Vector2 _size;
    private string _tagToAdd;
    #region House
    [SerializeField] private GameObject _habitation;
    Vector2 _habitationSize = new Vector2(2, 2);
    #endregion
    #region Storage
    [SerializeField] private GameObject _reserve;
    Vector2 _reserveSize = new Vector2(3, 2);
    #endregion
    #region Field
    [SerializeField] private GameObject _champs;
    Vector2 _champsSize = new Vector2(3, 3);
    #endregion
    #region LandingZone
    [SerializeField] private GameObject _zoneAtterissage;
    Vector2 _zoneAtterissageSize = new Vector2(3, 7);
    #endregion

    private void Start()
    {
        _marketMenu.SetActive(false);
    }

    private void Update()
    {
        if (_currentConstruction != null)
        {
            GetCellCenterPosition();
            _currentConstruction.transform.position = cellCenterPosition;
            
            if (Input.GetMouseButtonDown(0))
            {
                constructionPosition = SpawnPosition(tilemap, out canBuild,_size.x,_size.y);
                if (canBuild)
                {
                    _currentConstruction.tag = _tagToAdd;
                    _currentConstruction = null;
                } 
            }
        }
    }

    private void GetCellCenterPosition()
    {
        camera = Camera.main;
        if (camera != null) worldPoint = camera.ScreenToWorldPoint(Input.mousePosition);
        cellPosition = tilemap.WorldToCell(worldPoint);
        cellCenterPosition = tilemap.GetCellCenterWorld(cellPosition);
    }
    
    private Vector3 SpawnPosition(Tilemap tilemap, out bool _canBuild,float sizeX , float sizeY )
    {
        GetCellCenterPosition();
        bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
        if (tilemap.HasTile(cellPosition) && !isOverUI)
        {
            // Check if there is already wood on this tile
            Collider2D[] colliders = Physics2D.OverlapAreaAll(cellCenterPosition, new Vector2(cellCenterPosition.x+sizeX,cellCenterPosition.y+sizeY));
            foreach (var collider in colliders)
            {
                GameObject collision = collider.gameObject;
                if (collision.CompareTag("Wood") || collision.CompareTag("Rock") || collision.CompareTag("Mountain") ||
                    collision.CompareTag("Meat")  || collision.CompareTag("House") || collision.CompareTag("Storage") || 
                    collision.CompareTag("Field")   || collision.CompareTag("LandingZone"))
                {
                    _canBuild = false;
                    return Vector3.zero;
                }
            }

            _canBuild = true;
            return cellCenterPosition;
        }

        _canBuild = false;
        return Vector3.zero;
    }
    public void ActiveMenu()
    {
        isActive = !isActive;
        _marketMenu.SetActive(isActive);
        _spawningMenu.DisableAllRessource();
    }

    public void CloseConstructionMenu()
    {
        isActive = false;
        _marketMenu.SetActive(isActive);
    }

    public void Habitation()
    {
        Debug.Log("Habitation");
        Vector3 mousePosition = Input.mousePosition;
        if (Camera.main != null)
        {
            mousePosition.z = Camera.main.nearClipPlane;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            _currentConstruction = Instantiate(_habitation, worldPosition, Quaternion.identity, _constructionParent);
            _size = _habitationSize;
            _tagToAdd = "House";
        }
    }
    public void Reserve()
    {
        Debug.Log("Reserve");
        Vector3 mousePosition = Input.mousePosition;
        if (Camera.main != null)
        {
            mousePosition.z = Camera.main.nearClipPlane;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            _currentConstruction = Instantiate(_reserve, worldPosition, Quaternion.identity, _constructionParent);
            _size = _reserveSize;
            _tagToAdd = "Storage";
        }
    }
    public void Champs()
    {
        Debug.Log("Field");
        Vector3 mousePosition = Input.mousePosition;
        if (Camera.main != null)
        {
            mousePosition.z = Camera.main.nearClipPlane;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            _currentConstruction = Instantiate(_champs, worldPosition, Quaternion.identity, _constructionParent);
            _size = _champsSize;
            _tagToAdd = "Field";
        }
    }
    public void ZoneAtterissage()
    {
        Debug.Log("LandingZone");
        Vector3 mousePosition = Input.mousePosition;
        if (Camera.main != null)
        {
            mousePosition.z = Camera.main.nearClipPlane;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            _currentConstruction = Instantiate(_zoneAtterissage, worldPosition, Quaternion.identity, _constructionParent);
            _size = _zoneAtterissageSize;
            _tagToAdd = "LandingZone";
        }
    }
}