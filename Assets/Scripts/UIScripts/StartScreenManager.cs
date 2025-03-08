using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartScreenManager : MonoBehaviour
{
    public AudioSource startSound; // Assign this in the Inspector

    public void StartGame()
    {
        if (startSound != null)
        {
            StartCoroutine(PlaySoundAndLoadScene()); // Start coroutine to wait for sound
        }
        else
        {
            SceneManager.LoadScene("Room_1"); // Load immediately if no sound
        }
    }

    IEnumerator PlaySoundAndLoadScene()
    {
        startSound.Play(); // Play the sound effect
        yield return new WaitForSeconds(startSound.clip.length); // Wait for the sound to finish
        SceneManager.LoadScene("Room_1"); // Load the next scene
    }
}