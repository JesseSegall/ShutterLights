using UnityEngine;

public class LightDecay : MonoBehaviour
{
    public float initialIntensity = 20f;
    private Light areaLight;
   // Singleton
    private LightDecayStatusBar lightDecayStatus;

    private void Start()
    {
        areaLight = GetComponent<Light>();
        // Use the singleton instance so we dont get dupes
        lightDecayStatus = LightDecayStatusBar.Instance;

        if (areaLight != null)
        {
            areaLight.intensity = initialIntensity;
        }

        if (lightDecayStatus != null)
        {
            Debug.Log(" Found LightDecayStatusBar");
        }
        else
        {
            Debug.LogWarning(" LightDecayStatusBar not found in the scene.");
        }
    }

    private void Update()
    {
        if (areaLight != null && lightDecayStatus != null)
        {
            float ratio = lightDecayStatus.GetCurrentRatio();
            areaLight.intensity = initialIntensity * ratio;

            if (ratio <= 0f)
            {
                if (areaLight.enabled)
                {
                    Debug.Log(" Disabling area light.");
                }
                areaLight.enabled = false;
            }
            else
            {
                if (!areaLight.enabled)
                {
                    Debug.Log(" Enabling area light.");
                }
                areaLight.enabled = true;
            }
        }
    }
}