using UnityEngine;

public class LightDecay : MonoBehaviour
{
    public float initialIntensity = 5f;
    private Light areaLight;
    private float timer;
    public float decayDuration = 15f;
    private LightDecayStatusBar lightDecayStatus; // Reference to the status bar

    private void Start()
    {
        areaLight = GetComponent<Light>();
        lightDecayStatus = FindObjectOfType<LightDecayStatusBar>(); // Get the status bar script

        if (areaLight != null)
        {
            areaLight.intensity = initialIntensity;
        }
    }

    private void Update()
    {
        if (areaLight != null && lightDecayStatus != null)
        {
            // Sync intensity with the status bar's fill amount
            float ratio = lightDecayStatus.GetCurrentRatio();
            areaLight.intensity = initialIntensity * ratio;

            if (ratio <= 0f)
            {
                areaLight.enabled = false; // Turn off the light when depleted
            }
            else
            {
                areaLight.enabled = true; // Keep light active if bar isn't empty
            }
        }
    }
    //public void GhostContactAreaLight(float damageAmount)
        //{
            //timer += damageAmount;
            //float ratio = Mathf.Clamp01(1 - (timer / decayDuration));

            //areaLight.intensity = initialIntensity * ratio;

            //if (ratio <= 0f)
                //{
                   // areaLight.enabled = false;
                //}
        //}
}