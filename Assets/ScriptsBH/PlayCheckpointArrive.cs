using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayCheckpointArrive : MonoBehaviour
{
public GameObject rightTorch;
public GameObject leftTorch;
private AudioSource audioSource;
public AudioClip checkpointSound;
private bool firstTime = false;
public TMP_Text checkpointText;

    private void Start(){
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if it's the player by tag directly on the colliding object or its parent
        if ((other.gameObject.CompareTag("Player") ||
            (other.transform.parent != null && other.transform.parent.CompareTag("Player"))) && !firstTime)
        {
            Light rightLight = rightTorch.GetComponent<Light>();
            Light leftLight = leftTorch.GetComponent<Light>();
            rightLight.enabled = true;
            leftLight.enabled = true;
            audioSource.PlayOneShot(checkpointSound);
            firstTime = true;
            if (checkpointText != null)
            {
                StartCoroutine(ShowCheckpointMessage());
            }
        }
    }

    private IEnumerator ShowCheckpointMessage()
    {
        checkpointText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        checkpointText.gameObject.SetActive(false);
    }
}
