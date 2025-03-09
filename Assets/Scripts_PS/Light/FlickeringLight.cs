using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public Light flickerLight;
    public float minIntensity = 0f;
    public float maxIntensity = 10.0f;
    public float flickerSpeed = 1.0f; // Speed of flickering

    private float targetIntensity;

    void Start()
    {
        if (flickerLight == null)
            flickerLight = GetComponent<Light>();

        targetIntensity = Random.Range(minIntensity, maxIntensity);
    }

    void Update()
    {
        flickerLight.intensity = Mathf.Lerp(flickerLight.intensity, targetIntensity, Time.deltaTime * flickerSpeed);

        if (Mathf.Abs(flickerLight.intensity - targetIntensity) < 0.1f)
        {
            targetIntensity = Random.Range(minIntensity, maxIntensity);
        }
    }
}