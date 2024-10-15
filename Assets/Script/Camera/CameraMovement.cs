using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
    public int speed;
    public int mapSize;
    private Vector3 dragOrigin;
    public float edgeDist = 10f;
    private new Camera camera;
    public float minZoom, maxZoom;
    private GameObject _mapVisualizer;

    private void Start()
    {
        camera = Camera.main;
        _mapVisualizer = transform.GetChild(0).gameObject;
    }

    private void CameraKeyboardMove()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0);
        MoveCamera(movement);
    }

    private void CameraMouseDragMove()
    {
        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(1)) return;

        Vector3 difference = Input.mousePosition - dragOrigin;
        dragOrigin = Input.mousePosition;

        Vector3 move = new Vector3(-difference.x * speed * Time.deltaTime, -difference.y * speed * Time.deltaTime, 0);
        MoveCamera(move);
    }

    private void CameraEdgeScroll()
    {
        Vector3 move = Vector3.zero;

        if (Input.mousePosition.x >= Screen.width - edgeDist)
        {
            move.x += speed * Time.deltaTime;
        }

        if (Input.mousePosition.x <= edgeDist)
        {
            move.x -= speed * Time.deltaTime;
        }

        if (Input.mousePosition.y >= Screen.height - edgeDist)
        {
            move.y += speed * Time.deltaTime;
        }

        if (Input.mousePosition.y <= edgeDist)
        {
            move.y -= speed * Time.deltaTime;
        }

        MoveCamera(move);
    }

    private void MoveCamera(Vector3 move)
    {
        Vector3 newPosition = transform.position + move;

        float cameraHeight = 2f * camera.orthographicSize;
        float cameraWidth = cameraHeight * camera.aspect;
        newPosition.x = Mathf.Clamp(newPosition.x, cameraWidth / 2, mapSize - cameraWidth / 2);
        newPosition.y = Mathf.Clamp(newPosition.y, cameraHeight / 2, mapSize - cameraHeight / 2);
        transform.position = newPosition;
    }

    private void ZoomCamera()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            camera.orthographicSize = Mathf.Clamp(camera.orthographicSize - (scroll * speed), minZoom, maxZoom);
            _mapVisualizer.transform.localScale = new Vector3(camera.orthographicSize/5, camera.orthographicSize/5, 1);
        }
    }

    private void Update()
    {
        CameraEdgeScroll();
        CameraKeyboardMove();
        CameraMouseDragMove();
        ZoomCamera();
    }
}