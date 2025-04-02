using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{

    public AudioClip launchSound;
    public AudioClip playerImpactSound;
    private AudioSource audioSource;
    void Awake() {
        audioSource = GetComponent<AudioSource>();
        if(audioSource == null) {
            Debug.Log("Projectile Audio is null");
        }
        audioSource.PlayOneShot(launchSound);
    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("player collision");
            audioSource.PlayOneShot(playerImpactSound);
            LightDecayStatusBar lightBar = collision.gameObject.GetComponentInChildren<LightDecayStatusBar>();
            LightDecay lightArea = collision.gameObject.GetComponentInChildren<LightDecay>();
            lightBar.DamageTaken(4f);
            Invoke("DestroyProjectile", 1.0f);
            //lightArea.GhostContactAreaLight(2f);
        }
        if(collision.gameObject.CompareTag("Terrain")) {
            Invoke("DestroyProjectile", 1.0f);
        }
    }

    private void DestroyProjectile() {
        Debug.Log("Destroying Projectile");
        Destroy(gameObject);
    }
}
