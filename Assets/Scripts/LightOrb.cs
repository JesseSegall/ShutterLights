using UnityEngine;

public class LightOrb : MonoBehaviour
{
    // How much "time" to subtract from the decay timer (i.e., how much to boost the light level)
    public float boostTime = 5f;

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
            Destroy(gameObject);
        }
    }
}