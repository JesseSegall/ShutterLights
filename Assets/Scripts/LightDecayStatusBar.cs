using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Required for scene reloading

public class LightDecayStatusBar : MonoBehaviour
{
    public static LightDecayStatusBar Instance; // Singleton instance

    public float decayDuration = 30f;
    public Image lightStatusBar;
    public YouDiedScreenManager youDiedManager;

    private float timer = 0f;
    private bool gameOverTriggered = false;
    

    private void Awake()
    {
        // Check to make sure if an old light bar exists its destroyed
        if (Instance != null && Instance != this)
        {
            Debug.Log("[LightDecayStatusBar] Duplicate instance detected, destroying new one.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        ResetLightStatusBar();
        Debug.Log("[LightDecayStatusBar] Start: Light status bar reset.");
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
            Debug.Log("[LightDecayStatusBar] Light fully decayed. Triggering game over.");

            if (PlayerDeath.Instance != null)
            {
                PlayerDeath.Instance.TriggerDeath();
                Debug.Log("[LightDecayStatusBar] PlayerDeath triggered.");
            }

            if (youDiedManager != null)
            {
                youDiedManager.ShowYouDiedScreen();
                Debug.Log("[LightDecayStatusBar] You Died screen shown.");
            }
        }
    }

    public void IncreaseLight(float boostTime)
    {
       // float before = timer;
        timer = Mathf.Max(0f, timer - boostTime);
       // Debug.Log($"[LightDecayStatusBar] IncreaseLight called. Timer before: {before:F2}, after: {timer:F2}");
        if (lightStatusBar != null)
        {
            lightStatusBar.fillAmount = Mathf.Clamp01(1 - (timer / decayDuration));
        }
        gameOverTriggered = false; // Allow revival if light is refilled
    }

    public void DamageTaken(float damageAmount)
    {
      ;
        timer += damageAmount;
        float ratio = Mathf.Clamp01(1 - (timer / decayDuration));
        if (lightStatusBar != null)
        {
            lightStatusBar.fillAmount = ratio;
        }


        if (ratio <= 0f && !gameOverTriggered)
        {
            gameOverTriggered = true;
            Debug.Log("[LightDecayStatusBar] Light fully decayed due to damage. Triggering game over.");

            if (PlayerDeath.Instance != null)
            {
                PlayerDeath.Instance.TriggerDeath();
                Debug.Log("[LightDecayStatusBar] PlayerDeath triggered.");
            }

            if (youDiedManager != null)
            {
                youDiedManager.ShowYouDiedScreen();
                Debug.Log("[LightDecayStatusBar] You Died screen shown.");
            }
        }
    }

    public float GetCurrentRatio()
    {
        return Mathf.Clamp01(1 - (timer / decayDuration));
    }

    // Resets the timer and fill amount for the light bar.
    public void ResetLightStatusBar()
    {

        timer = 0f;
        gameOverTriggered = false;
        if (lightStatusBar != null)
        {
            lightStatusBar.fillAmount = 1f;
        }
        //Debug.Log("[LightDecayStatusBar] Light status bar has been reset to full.");
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Debug.Log("[LightDecayStatusBar] OnDestroy: Unsubscribed from sceneLoaded event.");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[LightDecayStatusBar] OnSceneLoaded called for scene: {scene.name}");

        //Get the status bar ref from new scene
        GameObject lightStatusBarObject = GameObject.FindWithTag("LightStatusBarUI");
        if (lightStatusBarObject != null)
        {
            lightStatusBar = lightStatusBarObject.GetComponent<Image>();
            Debug.Log("[LightDecayStatusBar] Updated lightStatusBar reference from new scene.");
        }
        else
        {
            Debug.LogWarning("[LightDecayStatusBar] Could not find LightStatusBar UI element in the new scene.");
        }
        ResetLightStatusBar();
    }
}
