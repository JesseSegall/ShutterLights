using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Required for scene reloading

public class LightDecayStatusBar : MonoBehaviour
{
    public float initialIntensity = 5f;
    public float decayDuration = 15f;
    public Image lightStatusBar;
    public YouDiedScreenManager youDiedManager;

    private float timer = 0f;
    private bool gameOverTriggered = false;

    private void Start()
    {
        ResetLightStatusBar(); // Reset light on load

        // Subscribe to scene load event to reset light
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        float ratio = Mathf.Clamp01(1 - (timer / decayDuration));

        if (lightStatusBar != null)
        {
            lightStatusBar.fillAmount = ratio;
        }

        if (ratio <= 0f && !gameOverTriggered)
        {
            gameOverTriggered = true;

            // **Trigger Player Death when light runs out**
            if (PlayerDeath.Instance != null)
            {
                PlayerDeath.Instance.TriggerDeath();
            }

            // Show "You Died" screen if applicable
            if (youDiedManager != null)
            {
                youDiedManager.ShowYouDiedScreen();
            }
        }
    }

    public void IncreaseLight(float boostTime)
    {
        timer = Mathf.Max(0f, timer - boostTime);
        if (lightStatusBar != null)
        {
            lightStatusBar.fillAmount = Mathf.Clamp01(1 - (timer / decayDuration));
        }

        gameOverTriggered = false; // Allow revival if light is refilled
    }

    public void GhostContact(float damageAmount)
    {
        timer += damageAmount;
        float ratio = Mathf.Clamp01(1 - (timer / decayDuration));

        if (lightStatusBar != null)
        {
            lightStatusBar.fillAmount = ratio;
        }

        if (ratio <= 0f && !gameOverTriggered)
        {
            gameOverTriggered = true;

            if (PlayerDeath.Instance != null)
            {
                PlayerDeath.Instance.TriggerDeath();
            }

            if (youDiedManager != null)
            {
                youDiedManager.ShowYouDiedScreen();
            }
        }
    }

    public float GetCurrentRatio()
    {
        return Mathf.Clamp01(1 - (timer / decayDuration));
    }

    // 🔄 **Reset Light Status on Player Death**
    public void ResetLightStatusBar()
    {
        timer = 0f; // Reset timer
        gameOverTriggered = false; // Reset death state

        if (lightStatusBar != null)
        {
            lightStatusBar.fillAmount = 1f; // Fully refill light bar
        }

        Debug.Log("Light status bar has been reset.");
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe when destroyed
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetLightStatusBar(); // Reset light when scene reloads
    }
}