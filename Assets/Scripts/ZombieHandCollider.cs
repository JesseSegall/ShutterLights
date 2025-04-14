using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ZombieHandCollider : MonoBehaviour
{
    public int damageAmount = 5;
    private bool _canDealDamage = true;
    public AudioSource handColliderAudioSource;
    public AudioClip[] playerDamageClips;
    public AudioClip punchSound;


    private void Start()
    {
        handColliderAudioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_canDealDamage && other.CompareTag("Player"))
        {
            // Deal damage to player
            LightDecayStatusBar lightBar = other.GetComponentInChildren<LightDecayStatusBar>();
            if (lightBar != null)
            {
                lightBar.DamageTaken(damageAmount);
                handColliderAudioSource.PlayOneShot(punchSound);
                Debug.Log("Sound should play");
                Debug.Log("Hand hit player for " + damageAmount + " damage!");
                StartCoroutine(PlayerDamageCooldown(0.3f));

                // Prevent multiple hits in rapid succession
                StartCoroutine(DamageCooldown());
            }
        }
    }

    private IEnumerator DamageCooldown()
    {
        _canDealDamage = false;
        // Wait for len of animation so hand doesnt cause dam when not in animation
        yield return new WaitForSeconds(1.0f);
        _canDealDamage = true;
    }

    private IEnumerator PlayerDamageCooldown(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (playerDamageClips.Length > 0)
        {
            Debug.Log("Inside dam cooldown coroutine");
            AudioClip randomDamageSound = playerDamageClips[Random.Range(0, playerDamageClips.Length)];
            handColliderAudioSource.PlayOneShot(randomDamageSound);
        }
       
    }
}