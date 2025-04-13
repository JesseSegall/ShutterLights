using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public static PlayerDeath Instance { get; private set; }

    private void Awake()
    {
        Instance = this; // Singleton for easy access
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "KillTrigger")
        {
            TriggerDeath();
        }
    }

    public void TriggerDeath() // This function will be called when light reaches 0
    {
        Debug.Log("Player has died due to darkness!");

        // Reset the light status bar on death
        LightDecayStatusBar lightDecay = FindObjectOfType<LightDecayStatusBar>();
        if (lightDecay != null)
        {
            lightDecay.ResetLightStatusBar(); // Reset the light bar
        }
        ScoreManager.instance.AddScore(-100);


        RespawnAtCheckpoint();
    }

    private void RespawnAtCheckpoint()
    {
        if (PlayerManager.Instance == null)
        {
            Debug.Log("Player Manager is null something went wrong!");
            return;
        }
        // Before respawning, force detach the player from any platform
        PlatformAttach platformAttach = GetComponent<PlatformAttach>();
        if (platformAttach != null)
        {
            // Need this or else player will still be a child of platform if they fell and didnt hit jump
            platformAttach.DetachIfAttached();
            Debug.Log("Detached from platform via death script.");
        }
        else
        {
            Debug.LogWarning("No PlatformAttach component found on the player.");
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