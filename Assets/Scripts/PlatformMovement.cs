using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("How far the platform moves from its starting position (in world units)")]
    public float moveDistance = 3f;

    [Tooltip("How fast the platform moves")]
    public float moveSpeed = 2f;

    [Tooltip("The axis along which the platform moves")]
    public Vector3 moveDirection = Vector3.right;

    [Header("Offset Settings")]
    [Tooltip("Phase offset in radians (0-6.28)")]
    [Range(0f, 6.28f)]
    public float phaseOffset = 0f;

    [Tooltip("Initial time offset in seconds")]
    public float timeOffset = 0f;

    private Vector3 _startPosition;


    void Start()
    {
        _startPosition = transform.position;

        // Normalize the direction vector, this converts it to a vector that points in the same direction but has a length of exactly one
        moveDirection = moveDirection.normalized;
    }

    void Update()
    {
        // Calculate the movement offset using a sine wave with phase offset and time offset
        float offset = Mathf.Sin((Time.time + timeOffset) * moveSpeed + phaseOffset) * moveDistance;

        // Update the platform's position relative to its starting position
        transform.position = _startPosition + moveDirection * offset;
    }

    // Makes a little gizmo so we can see its path in editor
    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
        {
            // Can customize gizmo, change color etc
            Gizmos.color = Color.cyan;
            Vector3 startPos = transform.position;
            Vector3 moveDir = moveDirection.normalized;

            // Draw line indicating movement path
            Gizmos.DrawLine(startPos - moveDir * moveDistance, startPos + moveDir * moveDistance);

            // Draw spheres at movement extremes
            Gizmos.DrawSphere(startPos - moveDir * moveDistance, 0.2f);
            Gizmos.DrawSphere(startPos + moveDir * moveDistance, 0.2f);
        }
    }
}