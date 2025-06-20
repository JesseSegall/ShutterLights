using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TutorialGuide : MonoBehaviour
{
    public GameObject tutorialPanel;
    public TMP_Text tutorialText;
    public Button nextButton;
    public Button exitButton;
    public GameObject arrowPointer;
    public GameObject wayPointOne;
    public GameObject refillObject;
    public GameObject greenOrb;
    public GameObject redOrb;
    public GameObject areaLight;
    public GameObject chalice;
    private LightDecayStatusBar script;
    private bool tutorialVisible = true;
    public bool tutorialOver = false;
    private Dictionary<int, Func<bool>> delayConditions;
    private HashSet<int> delayedSteps = new HashSet<int>();
    private bool redOrbCollected = false;
    private bool greenOrbCollected = false;
    private Coroutine delayCoroutine = null;
    

    private string[] tutorialSteps = {
        "Welcome to the game!",
        "Use WASD or Arrow keys to move to the arrow",
        "Use the mouse to look around.",
        "Press Space to jump onto the elevator.",
        "Press Q to pause the game",
        "Notice your health bar is decreasing over time",
        "To refill your health bar and gain points collect light orbs",
        "There are also power ups",
        "Go collect the green orb",
        "Notice your jump is much higher",
        "Go collect the red orb",
        "Notice you can now run faster",
        "Now go collect the gold chalice",
        "You earn a lot of points by finding these chalices",
        "Enter level one by moving over the light orange portal at the end of the hallway"
    };

    public int currentStep = 0;

    void Start()
    {   
        string spawnPointID = PlayerManager.Instance?.CurrentSpawnPointID;
        Debug.Log("Tutotrial Script: Spawn Point ID: " + spawnPointID);
        script = LightDecayStatusBar.Instance;
        redOrbCollected = false;
        greenOrbCollected = false;

        if (spawnPointID == "SpawnPoint_MenuRespawn" || string.IsNullOrEmpty(spawnPointID))
        {
            PlayerPrefs.DeleteKey("FromStartScene");
            tutorialPanel.SetActive(true);
            ShowStep(currentStep);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            

            script.enabled = false;
            float scaleFactor = Screen.height / 1080f;
            tutorialText.fontSize = 60 * scaleFactor;

            TMP_Text nextText = nextButton.GetComponentInChildren<TMP_Text>();
            TMP_Text exitText = exitButton.GetComponentInChildren<TMP_Text>();

            if (nextText != null) nextText.fontSize = 60 * scaleFactor;
            if (exitText != null) exitText.fontSize = 60 * scaleFactor;
            }
        else {
            tutorialPanel.SetActive(false);
            this.enabled = false;
        }

        delayConditions = new Dictionary<int, Func<bool>>()
        {
            { 2,  () => true },
            { 4,  () => Input.GetKeyDown(KeyCode.Q) },
            { 5,  () => true },
            { 7,  () => true },
            { 9,  () => Input.GetKeyDown(KeyCode.Space) && greenOrbCollected},
            { 11, () => (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && redOrbCollected},
            { 13, () => true },
            { tutorialSteps.Length - 1, () => true }
        };
    }

    void Update(){
        //if (script != null && script.GetInstanceID() != 0) {
        //UpdateLightBar(script);
        //} else if (areaLight != null) {
            //script = areaLight.GetComponent<LightDecayStatusBar>();
        //}
        UpdateArrow();
        UpdateLightBar(script);
        if (!tutorialOver && delayConditions.ContainsKey(currentStep) && !delayedSteps.Contains(currentStep) && delayCoroutine == null)
            {
                if (delayConditions[currentStep]())
                {
                    delayedSteps.Add(currentStep);
                    delayCoroutine = StartCoroutine(DelayNextStep());
                }
            }
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //ToggleTutorial();
        //}
        //if (currentStep >= tutorialSteps.Length) {
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
        //}
        if (Input.GetKeyDown(KeyCode.E)){
            if (delayCoroutine != null)
                {
                    StopCoroutine(delayCoroutine);
                    delayCoroutine = null;
                }
            NextStep();
        }
        if (Input.GetKeyDown(KeyCode.X)){
            Exit();
        }
    }

    IEnumerator DelayNextStep(){
        yield return new WaitForSeconds(3f);
        delayCoroutine = null;
        NextStep();
    }

    public void ToggleTutorial(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        tutorialVisible = !tutorialVisible;
        tutorialPanel.SetActive(tutorialVisible);
    }


    void UpdateLightBar(MonoBehaviour script){
    //turn on lightbar decay when the right step has been reached
        if (currentStep == 5) {
            Debug.Log("Script is null? " + (script == null));
            Debug.Log("Script.gameObject is destroyed? " + (script != null && script.gameObject == null));
            script.enabled = true;
        }
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

        if (currentStep == 11 && !redOrb.GetComponent<MeshRenderer>().enabled) {
            redOrbCollected = true;
        }

        if (currentStep == 9 && !greenOrb.GetComponent<MeshRenderer>().enabled) {
            greenOrbCollected = true;
        }

        if (currentStep < tutorialSteps.Length)
        {
            ShowStep(currentStep);
        }
        else
        {
            //hide the panel to end the tutorial
            tutorialPanel.SetActive(false);
            arrowPointer.SetActive(false);
            tutorialOver = true;
        }
    }

    public void Exit()
    {
    Debug.Log("Exit button clicked!");
    //hide the panel to end the tutorial
    tutorialPanel.SetActive(false);
    //Cursor.lockState = CursorLockMode.Locked;
    //Cursor.visible = false;
    tutorialOver = true;
    if (!script.enabled){
        script.enabled = true;
    }
    }


    void UpdateArrow()
     {
        GameObject target = null;

        if (currentStep == 6) target = refillObject;
        else if (currentStep == 1) target = wayPointOne;
        else if (currentStep == 8) target = greenOrb;
        else if (currentStep == 10) target = redOrb;
        else if (currentStep == 12) target = chalice;


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
