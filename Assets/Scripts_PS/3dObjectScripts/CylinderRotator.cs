using UnityEngine;

public class CylinderRotator : MonoBehaviour
{
    // Rotation speed in degrees per second
    public float rotationSpeed = 90f;

    void Update()
    {
        // Rotate the cylinder around the Y-axis
        transform.Rotate(Vector3.down, rotationSpeed * Time.deltaTime);
    }
}