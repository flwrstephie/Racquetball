using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; 
    public Vector3 offset = new Vector3(0, 5, -10); 
    public float smoothSpeed = 0.125f; 

    [Header("Zoom Settings")]
    public float minZoom = 15f; 
    public float maxZoom = 60f; 
    public float zoomSpeed = 10f; 

    private Camera cam;

    void Start()
    {
    
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 desiredPosition = player.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            
            transform.position = smoothedPosition;
            transform.LookAt(player);
        }

        HandleZoom();
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (cam.orthographic == false)
        {
            cam.fieldOfView -= scroll * zoomSpeed;
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, minZoom, maxZoom);
        }
        
        else
        {
            cam.orthographicSize -= scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
    }
}
