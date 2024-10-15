using UnityEngine;
using UnityEngine.Tilemaps;

public class Spawning 
{
    public Vector3 SpawnPosition(Tilemap tilemap,out bool canBuild)
    {
        Camera camera = Camera.main;
        Vector3 worldPoint = camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = tilemap.WorldToCell(worldPoint);
        Vector3 cellCenterPosition = tilemap.GetCellCenterWorld(cellPosition);

        if (tilemap.HasTile(cellPosition))
        {
            // Check if there is already wood on this tile
            Collider2D[] colliders = Physics2D.OverlapCircleAll(cellCenterPosition, 0.1f);
            foreach (var collider in colliders)
            { 
                GameObject collision = collider.gameObject;
                if (collision.CompareTag("Wood") || collision.CompareTag("Rock") || collision.CompareTag("Mountain") || collision.CompareTag("Meat"))
                {
                    canBuild = false;
                    return Vector3.zero;
                }
            }
            canBuild = true;
            return cellCenterPosition;
        }

        canBuild = false;
        return Vector3.zero;
    }
}
