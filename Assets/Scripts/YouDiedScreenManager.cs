using UnityEngine;

public class YouDiedScreenManager : MonoBehaviour
{
    // Reference to the "You Died" UI Image or Panel.
    public GameObject youDiedImage;

    private void Start()
    {
        // Ensure the image is hidden at the start.
        if (youDiedImage != null)
        {
            youDiedImage.SetActive(false);
        }
    }

    // Call this method to display the "You Died" screen.
    public void ShowYouDiedScreen()
    {
        if (youDiedImage != null)
        {
            youDiedImage.SetActive(true);
        }
    }
}