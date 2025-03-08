using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public string CurrentSpawnPointID { get; private set; }
    private void Awake()
    {
        // Need to check if instance of PM exists, if it does we destroy so we do not have duplicates
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void SetSpawnPointID(string spawnPointID)
    {
        CurrentSpawnPointID = spawnPointID;
        Debug.Log("Current spawn ID is: " + CurrentSpawnPointID);
    }



    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (string.IsNullOrEmpty(CurrentSpawnPointID))
            return;

        //Get the player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
            return;

        // Get all spawn points
        SpawnPoint[] spawnPoints = FindObjectsOfType<SpawnPoint>();

        // Find the one matching our ID then spawn the player at that location
        foreach (SpawnPoint spawnPoint in spawnPoints)
        {
            if (spawnPoint.spawnPointID == CurrentSpawnPointID)
            {
                player.transform.position = spawnPoint.transform.position;
                player.transform.rotation = spawnPoint.transform.rotation;
                break;
            }
        }

    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
