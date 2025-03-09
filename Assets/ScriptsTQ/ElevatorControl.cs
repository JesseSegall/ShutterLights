using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorControl : MonoBehaviour
{
    public GameObject elevatorCollider;
    private Animator anim;
    private ElevatorStarter elevatorStarter;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        elevatorStarter = elevatorCollider.GetComponent<ElevatorStarter>();
        if (elevatorStarter == null)
        {
            Debug.Log("Elevator Starter is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (elevatorStarter.ElevatorTriggered)
        {
            anim.SetBool("StartElevator", true);
        }
    }
}
