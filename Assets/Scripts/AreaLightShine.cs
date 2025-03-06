using UnityEngine;

public class AreaLightShine : MonoBehaviour
{
    // Base intensity of the light
    public float baseIntensity = 1f;
    
    // Additional intensity added during the pulse
    public float pulseIntensity = 1f;
    
    // Speed of the pulsing effect
    public float pulseSpeed = 2f;
    
    private Light areaLight;
    
    private void Awake()
    {
        // Get the Light component attached to this GameObject
        areaLight = GetComponent<Light>();
    }
    
    private void Update()
    {
        if (areaLight != null)
        {
            // Create a pulsing effect using PingPong to vary intensity over time
            areaLight.intensity = baseIntensity + Mathf.PingPong(Time.time * pulseSpeed, pulseIntensity);
        }
    }
}