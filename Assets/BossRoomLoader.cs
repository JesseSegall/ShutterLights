using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossRoomLoader : MonoBehaviour
{
    public Renderer rend;
    [Header("Scene Transition Settings")]
    [Tooltip("Name of the scene to load")]
    public string destinationScene = "BossRoom";

    [Tooltip("ID of the spawn point to use in the destination scene")]
    public string destinationSpawnPointID = "MainToBoss";

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend == null)
        {
            Debug.Log("Renderer could not be found");
        }
        rend.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTrigger called for " + gameObject.name);
        Debug.Log("Collided with: " + other.gameObject.name);

        // Check if it's the player by tag directly on the colliding object or its parent
        if (other.gameObject.CompareTag("Player") ||
           (other.transform.parent != null && other.transform.parent.CompareTag("Player")))
        {
            Debug.Log("Got player tag");

            // Tell PlayerManager which spawn point to use
            if (PlayerManager.Instance != null)
            {
                Debug.Log("Setting spawn point ID to: " + destinationSpawnPointID);
                PlayerManager.Instance.SetSpawnPointID(destinationSpawnPointID);
            }
            else
            {
                Debug.LogError("PlayerManager.Instance is null. Did you create the PlayerManager object in the Main scene? There should only be one.");
            }
            LightDecayStatusBar statusBar = other.gameObject.GetComponentInChildren<LightDecayStatusBar>();
            statusBar.decayDuration = 25;

            Debug.Log("Loading scene: " + destinationScene);
            SceneManager.LoadScene(destinationScene);

        }
        else
        {
            Debug.Log("Not the player. GameObject: " + other.gameObject.name);
        }
    }


}
