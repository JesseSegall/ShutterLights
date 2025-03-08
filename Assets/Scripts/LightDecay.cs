using UnityEngine;

public class LightDecay : MonoBehaviour
{
    // The starting intensity of the light.
    public float initialIntensity = 5f;
    // How long (in seconds) it takes for the light to decay to 0.
    public float decayDuration = 15f;

    private Light areaLight;
    private float timer = 0f;

    private void Start()
    {
        // Get the Light component attached to this GameObject.
        areaLight = GetComponent<Light>();
        if (areaLight != null)
        {
            areaLight.intensity = initialIntensity;
        }
    }

    private void Update()
    {
        if (areaLight != null)
        {
            // Increase the timer with the elapsed time.
            timer += Time.deltaTime;

            // Calculate a ratio (1 when the boost starts, 0 when the time has elapsed).
            float ratio = Mathf.Clamp01(1 - (timer / decayDuration));

            // Update the light's intensity based on the ratio.
            areaLight.intensity = initialIntensity * ratio;

            // Optionally, disable the light when it fully decays.
            if (ratio <= 0f)
            {
                areaLight.enabled = false;
            }
        }
    }
}