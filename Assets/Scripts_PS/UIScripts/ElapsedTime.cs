using UnityEngine;
using TMPro;

public class ElapsedTime : MonoBehaviour
{
    public TextMeshProUGUI elapsedTimeText;
    private float elapsedTime;

    void Update()
    {
        elapsedTime += Time.deltaTime;
        UpdateTimeDisplay();
        UpdateTimeColor();
    }

    void UpdateTimeDisplay()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        elapsedTimeText.text = $"Time: {minutes:00}:{seconds:00}";
    }

    void UpdateTimeColor()
    {
        if (elapsedTime >= 240) // 4 minutes and beyond: Red
            elapsedTimeText.color = Color.red;
        else if (elapsedTime >= 120) // 2 minutes to 4 minutes: Yellow
            elapsedTimeText.color = Color.yellow;
        else // Start to 2 minutes: Green
            elapsedTimeText.color = Color.green;
    }
}