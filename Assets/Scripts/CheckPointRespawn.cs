using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointRespawn : MonoBehaviour {
    private bool _found;
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

                    Transform playerTransform = other.gameObject.CompareTag("Player") ?
                        other.transform : other.transform.parent;


                    playerTransform.position = spawnPoint.transform.position;
                    playerTransform.rotation = spawnPoint.transform.rotation;

                    Debug.Log("Player teleported to: " + targetSpawnPointID);
                    _found = true;
                    break;

                } if(!_found)
                {
                    Debug.Log($"Spawn point: {spawnPoint} not found, check that it is correct or it exists.");
                }
            }
        }
    }
}
