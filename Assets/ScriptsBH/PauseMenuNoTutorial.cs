using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;


[RequireComponent(typeof(CanvasGroup))]
public class PauseMenuNoTutorial : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject pauseMenuUI;
    public Button resumeButton;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = pauseMenuUI.GetComponent<CanvasGroup>();

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
                ResumeTheGame();
            }
            else
            {
                PauseTheGame();
            }
        }
    }

    public void PauseTheGame()
    {
        //pauseMenuUI.SetActive(true);
        isPaused = true;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        Time.timeScale = 0f;

       
        // Unlock the cursor for UI interaction
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);

    }

    public void ResumeTheGame()
    {
        //pauseMenuUI.SetActive(false);
        isPaused = false;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0f;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        
    }
        public void RestartTheGame()
    {
        Time.timeScale = 1f; // Reset time scale
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitTheGame()
    {
        Time.timeScale = 1f; // Reset time scale
        Application.Quit();
    }
}
