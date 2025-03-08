using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
 private void OnTriggerEnter(Collider other)
 {
  if (other.gameObject.tag == "KillTrigger")
  {
   RespawnAtCheckpoint();
  }
 }

 private void RespawnAtCheckpoint()
 {
  if (PlayerManager.Instance == null)
  {
   Debug.Log("Player Manager is null something went wrong!");
   return;
  }
  string spawnPointID = PlayerManager.Instance.CurrentSpawnPointID;

  if (string.IsNullOrEmpty(spawnPointID))
  {
   Debug.Log("Player SpawnPoint ID is null something went wrong!");
  }
  SpawnPoint[] spawnPoints = FindObjectsOfType<SpawnPoint>();
  foreach (SpawnPoint spawnPoint in spawnPoints)
  {
   if (spawnPoint.spawnPointID == spawnPointID)
   {
    transform.position = spawnPoint.transform.position;
    transform.rotation = spawnPoint.transform.rotation;

    Debug.Log("Player respawned at checkpoint: " + spawnPointID);
    break;
   }
  }
 }
}
