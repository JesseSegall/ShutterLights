using UnityEngine;
using System.Collections;

public class BossLightOrb : MonoBehaviour
{
    // How much "time" to subtract from the decay timer (i.e., how much to boost the light level)
    public float boostTime = 5f;
    public float respawnTime = 5f;
    private Vector3 spawnPosition;
    private BossBehavior bossScript;
    private GameObject boss;
    private MeshRenderer meshRenderer;
    private Collider orbCollider;

     private void Start()
    {
        spawnPosition = transform.position;
        boss = GameObject.FindWithTag("theboss"); 
        bossScript = boss.GetComponent<BossBehavior>();
        meshRenderer = GetComponent<MeshRenderer>();
        orbCollider = GetComponent<Collider>();
        
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
            meshRenderer.enabled = false;
            orbCollider.enabled = false;
            bossScript.bossHealth -= 10;
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