using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Tooltip("ID of the spawn point to teleport to")]
    public string targetSpawnPointID;

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player") ||
            (other.transform.parent != null && other.transform.parent.CompareTag("Player")))
        {
            Debug.Log("Player entered checkpoint. Teleporting to: " + targetSpawnPointID);


            SpawnPoint[] spawnPoints = FindObjectsOfType<SpawnPoint>();
            foreach (SpawnPoint spawnPoint in spawnPoints)
            {
                if (spawnPoint.spawnPointID == targetSpawnPointID)
                {
                    Transform playerTransform = null;

                    if (other.gameObject.CompareTag("Player"))
                    {
                        playerTransform = other.transform;
                    }
                    else if (other.transform.parent != null && other.transform.parent.CompareTag("Player"))
                    {
                        playerTransform = other.transform.parent;
                    }

                    if (playerTransform == null)
                    {
                        Debug.Log("Player tag not found.");
                        return;
                    }



                    playerTransform.position = spawnPoint.transform.position;
                    playerTransform.rotation = spawnPoint.transform.rotation;

                    Debug.Log("Player teleported to: " + targetSpawnPointID);

                    break;

                }
            }
        }
    }
}
