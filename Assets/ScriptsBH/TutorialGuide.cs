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
    public GameObject refillObject;
    public GameObject greenOrb;
    public GameObject redOrb;

    private string[] tutorialSteps = {
        "Welcome to the game!",
        "Use WASD or Arrow keys to move.",
        "Use the mouse to look around.",
        "Press Space to jump.",
        "Notice your health bar is decreasing over time",
        "To refill your health bar collect light orbs",
        "There are also power ups",
        "Go collect the green orb",
        "Notice your jump is much higher",
        "Go collect the red orb",
        "Notice you can now run faster",
        "That's it! Good luck!"
    };

    public int currentStep = 0;

    void Start()
    {
        ShowStep(currentStep);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update(){
        UpdateArrow();
    }

    void ShowStep(int step)
    {
        tutorialText.text = tutorialSteps[step];
        //if (step == 4 || step == 5 || step == 7 || step == 9)
        //{
            //arrowPointer.SetActive(true);
        //}
        //else
        //{
           // arrowPointer.SetActive(false);
        //}
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
            arrowPointer.SetActive(false);
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


    void UpdateArrow()
     {
        GameObject target = null;

        if (currentStep == 5) target = refillObject;
        else if (currentStep == 7) target = greenOrb;
        else if (currentStep == 9) target = redOrb;

        if (target != null && target.activeInHierarchy)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(target.transform.position);

            if (screenPosition.z > 0)
            {
                arrowPointer.SetActive(true);

                RectTransform arrowRect = arrowPointer.GetComponent<RectTransform>();
                if (arrowRect != null)
                {
                    arrowRect.position = screenPosition + new Vector3(0, 100, 0);
                }
                else
                {
                    arrowPointer.transform.position = screenPosition + new Vector3(0, 100, 0);
                }
            }
            else
            {
                arrowPointer.SetActive(false);
            }
        }
        else
        {
            arrowPointer.SetActive(false);
        }
    }

    public static TutorialGuide Instance;

    void Awake()
    {
        Instance = this;
    }
}
