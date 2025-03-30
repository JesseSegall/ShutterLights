using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHandCollider : MonoBehaviour
{
    public int damageAmount = 2;
    private bool canDealDamage = true;

    private void OnTriggerEnter(Collider other)
    {
        if (canDealDamage && other.CompareTag("Player"))
        {
            // Deal damage to player
            LightDecayStatusBar lightBar = other.GetComponentInChildren<LightDecayStatusBar>();
            if (lightBar != null)
            {
                lightBar.DamageTaken(damageAmount);
                Debug.Log("Hand hit player for " + damageAmount + " damage!");

                // Prevent multiple hits in rapid succession
                StartCoroutine(DamageCooldown());
            }
        }
    }

    private IEnumerator DamageCooldown()
    {
        canDealDamage = false;
        // Wait for len of animation so hand doesnt cause dam when not in animation
        yield return new WaitForSeconds(1.1f);
        canDealDamage = true;
    }
}