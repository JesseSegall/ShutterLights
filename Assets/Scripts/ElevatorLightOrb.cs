using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorLightOrb : MonoBehaviour
{
    // How much "time" to subtract from the decay timer (i.e., how much to boost the light level)
    public float boostTime = 5f;
    public float respawnTime = 5f;
    private Vector3 spawnPosition;
    private MeshRenderer meshRenderer;
    private Collider orbCollider;
    public AudioClip lightSound; 
    private AudioSource audioSource;
    public GameObject elevator;

     private void Start()
    {
        spawnPosition = transform.position;
        meshRenderer = GetComponent<MeshRenderer>();
        orbCollider = GetComponent<Collider>();
        audioSource = gameObject.AddComponent<AudioSource>();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ensure the orb only reacts to the player (adjust tag as needed)
        if (other.CompareTag("Player"))
        {
            // Find the LightDecayStatusBar in the scene.
            LightDecayStatusBar lightDecay = FindObjectOfType<LightDecayStatusBar>();
            if (lightDecay != null)
            {
                // Increase the light level by subtracting boostTime from the timer.
                lightDecay.IncreaseLight(boostTime);
            }
                        // Destroy the orb so it can only be used once.
            StartCoroutine(RespawnOrb());
            audioSource.PlayOneShot(lightSound);
            meshRenderer.enabled = false;
            orbCollider.enabled = false;
        }
    }

    private IEnumerator RespawnOrb()
    {
        yield return new WaitForSeconds(respawnTime);
        //Debug.Log("respawn");
        Vector3 elevatorPosition = elevator.transform.position;
        Vector3 offset = new Vector3(2f, 2f, 3.24f); 
        spawnPosition = elevatorPosition + offset;
        meshRenderer.enabled = true;
        orbCollider.enabled = true;
    }
}