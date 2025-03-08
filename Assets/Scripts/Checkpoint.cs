using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This script will set the spawnPointID to whatever the most recent checkpoint is.
/// </summary>
public class Checkpoint : MonoBehaviour
{
    [Tooltip("ID of the spawn point to set as current checkpoint")]
    public string checkpointSpawnPointID;

    private void OnTriggerEnter(Collider other)
    {
        // Check if it's the player by tag directly on the colliding object or its parent
        if (other.gameObject.CompareTag("Player") ||
            (other.transform.parent != null && other.transform.parent.CompareTag("Player")))
        {
            // Set the current checkpoint in PlayerManager
            if (PlayerManager.Instance != null)
            {
                // Set the cp ID in the PM
                PlayerManager.Instance.SetSpawnPointID(checkpointSpawnPointID);
                Debug.Log("Checkpoint activated: " + checkpointSpawnPointID);
            }
            else
            {
                Debug.LogError("PlayerManager.Instance is null. Make sure the PlayerManager exists in the scene.");
            }
        }
    }
}