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
            LightDecayStatusBar statusBar = other.gameObject.GetComponentInChildren<LightDecayStatusBar>();
            if(statusBar == null) {
                Debug.Log("Status bar could not be found");
            }
            else {
                Debug.Log("Status bar found");
            }
            statusBar.decayDuration = 1000;
        }
        else
        {
            Debug.Log("Not the player. GameObject: " + other.gameObject.name);
        }
    }
}
