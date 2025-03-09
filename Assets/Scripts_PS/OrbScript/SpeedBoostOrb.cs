using UnityEngine;
using UnityEngine.UI;  // For UI components
using System.Collections;
using StarterAssets;   // Ensure this matches the namespace of your ThirdPersonController

public class SpeedBoostOrb : MonoBehaviour
{
    // Boost settings for movement
    public float speedMultiplier = 1.5f;
    public float boostDuration = 15f;
    
    // Boost settings for animation speed (adjust separately)
    public float animSpeedMultiplier = 1.2f;  // Tweak this value to control animation speed boost

    // Reference to the UI Image representing the boost status bar.
    public Image boostStatusBar;

    // Static variables to help prevent stacking boosts
    private static Coroutine currentBoostCoroutine;
    private static float originalMoveSpeed;
    private static float originalSprintSpeed;
    
    private void Awake()
    {
        // If boostStatusBar isn't assigned in the Inspector, try to find it by name in the scene.
        if (boostStatusBar == null)
        {
            GameObject barObj = GameObject.Find("BoostStatusBar");
            if (barObj != null)
            {
                boostStatusBar = barObj.GetComponent<Image>();
            }
        }

        // Initialize the boost bar so it's empty and hidden at the start.
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
                // If a boost is already active, stop it and revert to the original speeds
                if (currentBoostCoroutine != null)
                {
                    controller.StopCoroutine(currentBoostCoroutine);
                    controller.MoveSpeed = originalMoveSpeed;
                    controller.SprintSpeed = originalSprintSpeed;
                }
                
                // Store original speeds before applying boost
                originalMoveSpeed = controller.MoveSpeed;
                originalSprintSpeed = controller.SprintSpeed;
                
                // Enable the boost status bar (it was hidden before)
                if (boostStatusBar != null)
                {
                    boostStatusBar.gameObject.SetActive(true);
                }
                
                // Start the boost coroutine and store its reference
                currentBoostCoroutine = controller.StartCoroutine(BoostSpeed(controller));
                
                // Optionally, destroy the orb so it can only be used once
                Destroy(gameObject);
            }
        }
    }
    
    private IEnumerator BoostSpeed(ThirdPersonController controller)
    {
        // Get the Animator component from the controller.
        Animator anim = controller.GetComponent<Animator>();
        float originalAnimSpeed = 1f;
        if (anim != null)
        {
            // Store the original animation speed and apply the animation boost multiplier.
            originalAnimSpeed = anim.speed;
            anim.speed = originalAnimSpeed * animSpeedMultiplier;
        }
        
        // Apply the boost to both move and sprint speeds.
        controller.MoveSpeed *= speedMultiplier;
        controller.SprintSpeed *= speedMultiplier;
        
        // Timer for the boost duration.
        float timer = boostDuration;
        while (timer > 0)
        {
            if (boostStatusBar != null)
            {
                // Update the fill amount proportionally (1 when boost starts, 0 when finished).
                boostStatusBar.fillAmount = timer / boostDuration;
            }
            timer -= Time.deltaTime;
            yield return null;
        }
        
        // Revert speeds to their original values.
        controller.MoveSpeed = originalMoveSpeed;
        controller.SprintSpeed = originalSprintSpeed;
        
        // Reset the animator's speed back to normal.
        if (anim != null)
        {
            anim.speed = originalAnimSpeed;
        }
        
        // Reset and hide the boost bar when the boost ends.
        if (boostStatusBar != null)
        {
            boostStatusBar.fillAmount = 0;
            boostStatusBar.gameObject.SetActive(false);
        }
        
        // Clear the boost coroutine flag.
        currentBoostCoroutine = null;
    }
}