using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnChalice : MonoBehaviour
{
    // How much "time" to subtract from the decay timer (i.e., how much to boost the light level)
    public float respawnTime = 5f;
    private Vector3 spawnPosition;
    private MeshRenderer meshRenderer;
    private Collider orbCollider;

     private void Start()
    {
        spawnPosition = transform.position;
        meshRenderer = GetComponent<MeshRenderer>();
        orbCollider = GetComponent<Collider>();

    }

    private void OnTriggerEnter(Collider other)
    {
        // Ensure the orb only reacts to the player (adjust tag as needed)
        if (other.CompareTag("Player"))
        {
            // Find the LightDecayStatusBar in the scene.
           
            StartCoroutine(RespawnOrb());
        }
    }

    private IEnumerator RespawnOrb()
    {
        yield return new WaitForSeconds(respawnTime);
        //Debug.Log("respawn");
        transform.position = spawnPosition;
        meshRenderer.enabled = true;
        orbCollider.enabled = true;
    }
}
