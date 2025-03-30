using UnityEngine;

public class MovingSpike : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public Transform pointC;
    public float speed = 2f;

    private Vector3 targetPosition;

    void Start()
    {
        PickRandomTarget();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            PickRandomTarget();
        }
    }

    void PickRandomTarget()
    {
        int randomPoint = Random.Range(0, 3);

        switch (randomPoint)
        {
            case 0:
                targetPosition = pointA.position;
                break;
            case 1:
                targetPosition = pointB.position;
                break;
            case 2:
                targetPosition = pointC.position;
                break;
        }
    }
}
