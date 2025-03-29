using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbDamage : MonoBehaviour
{
    public float damageAmount = 3f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit by Orb");
            LightDecayStatusBar lightBar = other.GetComponentInChildren<LightDecayStatusBar>();
            lightBar.DamageTaken(damageAmount);
            Destroy(gameObject);
        }
    }
}
