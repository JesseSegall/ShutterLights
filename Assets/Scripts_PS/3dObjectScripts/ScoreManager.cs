using UnityEngine;
using TMPro;  // Important: add this namespace for TextMeshPro

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public TextMeshProUGUI scoreText;  // Use TextMeshProUGUI for UI
    private int score = 0;

    void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateScoreText();
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = $"Score: {score}";
    }

    public void ResetScore(){
        score = 0;
        UpdateScoreText();
    }
}