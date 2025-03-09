using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorStarter : MonoBehaviour
{
    public bool ElevatorTriggered {get; set;}

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTrigger called for " + gameObject.name);
        Debug.Log("Collided with: " + other.gameObject.name);

        // Check if it's the player by tag directly on the colliding object or its parent
        if(other.gameObject.CompareTag("Player") ||
           (other.transform.parent != null && other.transform.parent.CompareTag("Player")))
        {
            Debug.Log("Got player tag");

            ElevatorTriggered = true;
            Debug.Log("elevatorTriggered is " + ElevatorTriggered);
        }
        else
        {
            Debug.Log("Not the player. GameObject: " + other.gameObject.name);
        }
    }
}
