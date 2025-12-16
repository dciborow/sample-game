using UnityEngine;

/// <summary>
/// Isometric camera that follows the player
/// </summary>
public class IsometricCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    
    [Header("Camera Settings")]
    public Vector3 offset = new Vector3(0, 10, -10);
    public float smoothSpeed = 5f;
    public float rotationAngle = 45f;
    
    void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
        }
        
        // Set isometric angle
        transform.rotation = Quaternion.Euler(rotationAngle, 0, 0);
    }
    
    void LateUpdate()
    {
        if (target == null)
            return;
            
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
