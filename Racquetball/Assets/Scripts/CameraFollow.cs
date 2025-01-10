using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Assign the player's Transform in the Inspector
    public Vector3 offset = new Vector3(0, 5, -10); // Offset for the camera position
    public float smoothSpeed = 0.125f; // Speed for smoothing

    [Header("Zoom Settings")]
    public float minZoom = 15f; // Minimum Field of View
    public float maxZoom = 60f; // Maximum Field of View
    public float zoomSpeed = 10f; // Speed of zooming

    private Camera cam;

    void Start()
    {
        // Get the Camera component
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (player != null)
        {
            // Desired position based on player's position and offset
            Vector3 desiredPosition = player.position + offset;

            // Smooth transition to the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Update the camera's position
            transform.position = smoothedPosition;

            // Optionally look at the player
            transform.LookAt(player);
        }

        HandleZoom();
    }

    void HandleZoom()
    {
        // Get mouse scroll wheel input
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // Adjust Field of View for perspective camera
        if (cam.orthographic == false)
        {
            cam.fieldOfView -= scroll * zoomSpeed;
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, minZoom, maxZoom);
        }
        // Adjust Orthographic Size for orthographic camera
        else
        {
            cam.orthographicSize -= scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
    }
}
