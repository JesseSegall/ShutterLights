using UnityEngine;

public class AxisRotator : MonoBehaviour
{
    public enum Axis { X, Y, Z }
    public Axis rotationAxis = Axis.Y;
    public float rotationSpeed = 90f; // degrees per second

    void Update()
    {
        Vector3 axisVector;

        switch(rotationAxis)
        {
            case Axis.X:
                axisVector = Vector3.right;
                break;
            case Axis.Y:
                axisVector = Vector3.up;
                break;
            case Axis.Z:
                axisVector = Vector3.forward;
                break;
            default:
                axisVector = Vector3.up;
                break;
        }

        transform.Rotate(axisVector, rotationSpeed * Time.deltaTime);
    }
}