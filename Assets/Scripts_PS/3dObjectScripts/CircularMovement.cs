using UnityEngine;

public class CircularMovement : MonoBehaviour
{
    private Vector3 centerPosition;  // The center of the circle will be set to the cube's initial position
    public float radius = 5f;        // How far from the center the cube moves
    public float rotationsPerSecond = 0.2f; // How fast the cube rotates

    void Start()
    {
        // Set the center to the cube's initial position
        centerPosition = transform.position;
    }

    void Update()
    {
        // Calculate the angle based on time
        float angle = rotationsPerSecond * Time.time * 2 * Mathf.PI;
        // Calculate new position relative to the center
        float x = centerPosition.x + Mathf.Cos(angle) * radius;
        float z = centerPosition.z + Mathf.Sin(angle) * radius;

        // Update the cube's position while keeping the original y-coordinate
        transform.position = new Vector3(x, transform.position.y, z);
    }
}