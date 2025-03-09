using UnityEngine;
using UnityEngine.UI;

public class LightDecayStatusBar : MonoBehaviour
{
    public float initialIntensity = 5f;
    public float decayDuration = 15f;
    public Image lightStatusBar;
    
    // Reference to the YouDiedScreenManager to show the game over screen.
    public YouDiedScreenManager youDiedManager;

    private Light areaLight;
    private float timer = 0f;
    private bool gameOverTriggered = false;

    private void Start()
    {
        areaLight = GetComponent<Light>();
        if (areaLight != null)
        {
            areaLight.intensity = initialIntensity;
        }
        if (lightStatusBar != null)
        {
            lightStatusBar.fillAmount = 1f;
        }
        // Ensure the game over image is hidden at start via the manager.
        if (youDiedManager != null)
        {
            youDiedManager.youDiedImage.SetActive(false);
        }
    }

    private void Update()
    {
        if (areaLight != null)
        {
            timer += Time.deltaTime;
            float ratio = Mathf.Clamp01(1 - (timer / decayDuration));
            areaLight.intensity = initialIntensity * ratio;
            if (lightStatusBar != null)
            {
                lightStatusBar.fillAmount = ratio;
            }
            if (ratio <= 0f && !gameOverTriggered)
            {
                areaLight.enabled = false;
                gameOverTriggered = true;
                // Show the You Died screen.
                if (youDiedManager != null)
                {
                    youDiedManager.ShowYouDiedScreen();
                }
            }
        }
    }

    public void IncreaseLight(float boostTime)
    {
        timer = Mathf.Max(0f, timer - boostTime);
        if (areaLight != null && !areaLight.enabled)
        {
            areaLight.enabled = true;
        }
        // Optionally hide the game over screen if using a revival mechanic.
    }

    public void GhostContact(float damageAmount)
    {
        timer += damageAmount; 
        float ratio = Mathf.Clamp01(1 - (timer / decayDuration));
        areaLight.intensity = initialIntensity * ratio;
        lightStatusBar.fillAmount = ratio;
        
        
        if (ratio <= 0f && !gameOverTriggered)
        {
            areaLight.enabled = false;
            gameOverTriggered = true;

            if (youDiedManager != null)
            {
                youDiedManager.ShowYouDiedScreen();
            }
        }
    }

}