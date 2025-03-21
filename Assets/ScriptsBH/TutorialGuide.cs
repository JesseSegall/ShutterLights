using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialGuide : MonoBehaviour
{
    public GameObject tutorialPanel;
    public TMP_Text tutorialText;
    public Button nextButton;
    public Button exitButton;
    public GameObject arrowPointer; 
    public Transform refillObject;

    private string[] tutorialSteps = {
        "Welcome to the game!",
        "Use WASD or Arrow keys to move.",
        "Use the mouse to look around.",
        "Press Space to jump.",
        "Notice your health bar is decreasing over time",
        "To refill your health bar collect light orbs",
        "That's it! Good luck!"
    };

    private int currentStep = 0;

    void Start()
    {
        ShowStep(currentStep);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void ShowStep(int step)
    {
        tutorialText.text = tutorialSteps[step];
        if (step == 4)
        {
            arrowPointer.SetActive(true);
        }
        else if (step == 5)
        {
            arrowPointer.SetActive(true);
            if (refillObject != null)
            {
                StartCoroutine(UpdateArrowPosition());
            }
        }
        else
        {
            arrowPointer.SetActive(false);
        }

    }

    public void NextStep()
    {
        Debug.Log("Next button clicked!");
        //loop through the steps so the player can learn the game
        currentStep++;

        if (currentStep < tutorialSteps.Length)
        {
            ShowStep(currentStep);
        }
        else
        {
            //hide the panel to end the tutorial
            tutorialPanel.SetActive(false);
        }
    }

    public void Exit()
    {
    Debug.Log("Exit button clicked!");
    //hide the panel to end the tutorial
    tutorialPanel.SetActive(false);
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
    }

    IEnumerator UpdateArrowPosition()
    {
        while (currentStep == 5 && )
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(refillObject.position);
            arrowPointer.transform.position = screenPosition + new Vector3(0, 50, 0);
            yield return null;
        }
    }
}
