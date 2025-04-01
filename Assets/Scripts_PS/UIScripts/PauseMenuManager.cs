using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;


[RequireComponent(typeof(CanvasGroup))]
public class PauseMenuManager : MonoBehaviour
{
    public CanvasGroup thecanvasGroup;
    private bool isPaused = false;
    public GameObject pauseMenuUI;
    public Button resumeButton;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = thecanvasGroup.GetComponent<CanvasGroup>();

        // Ensure menu is hidden at start
        pauseMenuUI.SetActive(true);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0f;
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.JoystickButton11))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        //pauseMenuUI.SetActive(true);
        isPaused = true;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        Time.timeScale = 0f;

        if (TutorialGuide.Instance != null && !TutorialGuide.Instance.tutorialOver){
            TutorialGuide.Instance.ToggleTutorial();
        }

        // Unlock the cursor for UI interaction
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);

    }

    public void ResumeGame()
    {
        //pauseMenuUI.SetActive(false);
        isPaused = false;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0f;
        Time.timeScale = 1f;

        if (TutorialGuide.Instance != null && !TutorialGuide.Instance.tutorialOver){
            TutorialGuide.Instance.ToggleTutorial();
        }
        

        // Lock the cursor back if needed
        if (TutorialGuide.Instance == null || !TutorialGuide.Instance.tutorialPanel.activeSelf)
        {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        }
    }
        public void RestartGame()
    {
        Time.timeScale = 1f; // Reset time scale
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f; // Reset time scale
        Application.Quit();
    }
}