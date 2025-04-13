using UnityEngine;
using UnityEngine.Tilemaps;
using System.Globalization;

public class Spawning
{
    public Vector2 StringToVector2(string value)
    {
        value = value.Replace("(", "").Replace(")", "");
        string[] vector = value.Split(',');
        Vector2 vector2 = new Vector2(float.Parse(vector[0], CultureInfo.InvariantCulture.NumberFormat),
            float.Parse(vector[1], CultureInfo.InvariantCulture.NumberFormat));
        return vector2;
    }

    public Vector3 SpawnPosition(Tilemap tilemap, out bool canBuild)
    {
        Camera camera = Camera.main;
        Vector3 worldPoint = camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = tilemap.WorldToCell(worldPoint);
        Vector3 cellCenterPosition = tilemap.GetCellCenterWorld(cellPosition);
        bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
        if (tilemap.HasTile(cellPosition) && !isOverUI)
        {
            // Check if there is already wood on this tile
            Collider2D[] colliders = Physics2D.OverlapCircleAll(cellCenterPosition, 0.1f);
            foreach (var collider in colliders)
            {
                GameObject collision = collider.gameObject;
                if (collision.CompareTag("Wood") || collision.CompareTag("Rock") || collision.CompareTag("Mountain") ||
                    collision.CompareTag("Meat") || collision.CompareTag("House") || collision.CompareTag("Storage") ||
                    collision.CompareTag("Field") || collision.CompareTag("LandingZone"))
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