using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is needed to keep the player and the camera across scenes, it must be attached to the player object and the cameras should be children of the player.
/// </summary>
public class PersistantObject : MonoBehaviour
{
    private static PersistantObject Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
