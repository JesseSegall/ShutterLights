using UnityEngine;
using UnityEngine.UI;      // For UI components
using System.Collections;
using StarterAssets;       // Ensure this matches the namespace of your ThirdPersonController

public class HighJumpOrb : MonoBehaviour
{
    // Boost settings for jump height
    public float jumpMultiplier = 2.0f;   // How many times higher the jump becomes
    public float boostDuration = 15f;     // Duration of the boost in seconds

    // Reference to a UI Image to display boost duration (optional)
    public Image boostStatusBar;

    // Static variables to help prevent stacking boosts
    private static Coroutine currentBoostCoroutine;
    private static float originalJumpHeight;
    
    private void Awake()
    {
        // If the boostStatusBar isn't set in the Inspector, try to find it by name
        if (boostStatusBar == null)
        {
            GameObject barObj = GameObject.Find("BoostStatusBar");
            if (barObj != null)
            {
                boostStatusBar = barObj.GetComponent<Image>();
            }
        }
        
        // Initialize the boost status bar so it is empty and hidden at the start.
        if (boostStatusBar != null)
        {
            boostStatusBar.fillAmount = 0;
            boostStatusBar.gameObject.SetActive(false);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ThirdPersonController controller = other.GetComponent<ThirdPersonController>();
            if (controller != null)
            {
                // If a boost is already active, stop it and revert to the original jump height
                if (currentBoostCoroutine != null)
                {
                    controller.StopCoroutine(currentBoostCoroutine);
                    controller.JumpHeight = originalJumpHeight;
                }
                
                // Store the player's original jump height before applying the boost
                originalJumpHeight = controller.JumpHeight;
                
                // Enable the boost status bar (if assigned)
                if (boostStatusBar != null)
                {
                    boostStatusBar.gameObject.SetActive(true);
                }
                
                // Start the boost coroutine and store its reference
                currentBoostCoroutine = controller.StartCoroutine(BoostJump(controller));
                
                // Destroy the orb so it can only be used once
                Destroy(gameObject);
            }
        }
    }
    
    private IEnumerator BoostJump(ThirdPersonController controller)
    {
        // Increase the player's jump height by the multiplier
        controller.JumpHeight *= jumpMultiplier;
        
        // Timer for the boost duration
        float timer = boostDuration;
        while (timer > 0)
        {
            if (boostStatusBar != null)
            {
                // Update the fill amount (1 when boost starts, 0 when finished)
                boostStatusBar.fillAmount = timer / boostDuration;
            }
            timer -= Time.deltaTime;
            yield return null;
        }
        
        // Revert the jump height back to the original value
        controller.JumpHeight = originalJumpHeight;
        
        // Reset and hide the boost status bar when the boost ends
        if (boostStatusBar != null)
        {
            boostStatusBar.fillAmount = 0;
            boostStatusBar.gameObject.SetActive(false);
        }
        
        // Clear the boost coroutine reference
        currentBoostCoroutine = null;
    }
}