using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialOrbTrigger : MonoBehaviour
{
    public int stepToTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && TutorialGuide.Instance != null)
        {
            if (TutorialGuide.Instance.currentStep == stepToTrigger)
            {
                Debug.Log($"Player triggered tutorial step {stepToTrigger} by hitting {gameObject.name}");
                TutorialGuide.Instance.NextStep();
            }
        }
    }
}
