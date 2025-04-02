using UnityEngine;

public class OpenGate : MonoBehaviour
{
    public GameObject gate;
    public float moveAmount = 5f;
    public float moveSpeed = 5f;
    public float activationDistance = 3f;

    private Vector3 initialGatePosition;
    private Vector3 targetGatePosition;
    private bool gateOpened = false;

    void Start()
    {
        initialGatePosition = gate.transform.position;
        targetGatePosition = initialGatePosition + Vector3.left * moveAmount;
    }

    void Update()
    {
        if (!gateOpened)
        {
            CheckPlayerProximity();
        }
        else
        {
            MoveGate();
        }
    }

    void CheckPlayerProximity()
    {
        if (TutorialGuide.Instance != null && TutorialGuide.Instance.tutorialOver)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null && Vector3.Distance(player.transform.position, transform.position) <= activationDistance)
            {
                gateOpened = true;
            }
        }
    }

    void MoveGate()
    {
        gate.transform.position = Vector3.MoveTowards(
            gate.transform.position,
            targetGatePosition,
            moveSpeed * Time.deltaTime
        );
    }
}