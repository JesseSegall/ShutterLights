
using UnityEngine;
/// <summary>
/// This script will match the decay rate of the spotlights with the area lights. The spotlights are what illuminate the player skin as the area light does not impact the player.
/// This will make it seem like the area light is lighting the player without causing unwanted shadows or other ill effects.
/// </summary>
public class SpotlightController : MonoBehaviour
{
    public Light[] spotlights;


    public float minimumSpotlightIntensity = 0.2f;

    public float maximumSpotlightIntensity = 5.0f;
    private LightDecayStatusBar _lightDecayStatus;
    // Start is called before the first frame update
    void Start()
    {   // Get instance of Status bar
        _lightDecayStatus = GetComponentInChildren<LightDecayStatusBar>();
        if (_lightDecayStatus == null)
        {
            Debug.LogError("No LightDecayStatusBar component found");
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (_lightDecayStatus != null && spotlights.Length > 0)
        {
            // Get current ratio and set the new intensity
            float currentRatio = _lightDecayStatus.GetCurrentRatio();
            float spotlightIntensity = minimumSpotlightIntensity + (maximumSpotlightIntensity - minimumSpotlightIntensity) * currentRatio;

            // Loop through the spotlights we have and assign new intensity to all in the array
            foreach (Light spotlight in spotlights)
            {
                if (spotlight != null)
                {
                    spotlight.intensity = spotlightIntensity;
                    spotlight.enabled = true;
                }
            }

        }
    }
}
