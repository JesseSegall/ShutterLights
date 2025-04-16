using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public static PlayerDeath Instance { get; private set; }
    public static bool HasPlayerDied = false;

    // Adding public event for zombies/boxes.
    public static event System.Action OnPlayerRespawn;

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
        HasPlayerDied = true;

        // Reset the light status bar on death.
        LightDecayStatusBar lightDecay = FindObjectOfType<LightDecayStatusBar>();
        if (lightDecay != null)
        {
            lightDecay.ResetLightStatusBar(); // Reset the light bar.
        }
        ScoreManager.instance.AddScore(-100);

        RespawnAtCheckpoint();
    }

    private void RespawnAtCheckpoint()
    {
        Debug.Log("Respawn at checkpoint called");
        if (PlayerManager.Instance == null)
        {
            Debug.Log("Player Manager is null; something went wrong!");
            return;
        }
        // Before respawning, force detach the player from any platform.
        PlatformAttach platformAttach = GetComponent<PlatformAttach>();
        if (platformAttach != null)
        {
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
            Debug.Log("Player SpawnPoint ID is null; something went wrong!");
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
        // This checks if there are any subs and if there are it will notify them
        OnPlayerRespawn?.Invoke();

        // Run the delay so subs have time to reset. Delays by just one frame
        StartCoroutine(ResetDeathFlagCoroutine());
    }

    private IEnumerator ResetDeathFlagCoroutine()
    {
        // This yield will have it wait one frame so nothing is racing and things have time to reset
        yield return null;  
        HasPlayerDied = false;
        Debug.Log("HasPlayerDied reset to: " + HasPlayerDied);
    }
}
