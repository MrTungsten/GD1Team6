using UnityEngine;
using TMPro; // Add this line for TextMeshPro
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{

    // Reference to the TextMeshProUGUI element to display the score
    [SerializeField] private TMP_Text scoreText;

    // Reference to the TextMeshProUGUI element to display the total score
    [SerializeField] private TMP_Text totalScoreText;

    // Reference to the TextMeshProUGUI element to display the high score
    [SerializeField] private TMP_Text highScoreText;

    // Dictionary to store score values for different GameObject types
    private Dictionary<string, int> scoreValues;

    // Variable to hold the current score
    private int currentScore = 0;

    // Variable to hold the total score
    private static int totalScore;

    // Variable to hold the high score
    private int highScore = 0;

    // Singleton pattern to ensure only one instance of ScoreManager exists
    public static ScoreManager Instance { get; private set; }

    private void Awake()
    {

        totalScoreText.text = "TotalScore\n" + totalScore.ToString();

        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Initialize the score values for different GameObject types
        scoreValues = new Dictionary<string, int>();
        scoreValues["EnemyDiver"] = 15;
        scoreValues["EnemyPlane"] = 5;
        scoreValues["EnemyTurret"] = 20;
        scoreValues["EnemyTank"] = 10;

        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateScoreText();
    }

    // Method to increment the score when a GameObject is destroyed
    public void IncrementScore(string gameObjectType)
    {
        if (scoreValues.ContainsKey(gameObjectType))
        {
            currentScore += scoreValues[gameObjectType];
            totalScore += scoreValues[gameObjectType];
            UpdateScoreText(); // Update the score text
            CheckAndUpdateHighScore();
        }
        else
        {
            Debug.LogWarning("No score value defined for " + gameObjectType);
        }
    }

    // Method to increment the score when the player picks up the score powerup
    public void ScorePowerup()
    {
        currentScore += 25;
        UpdateScoreText(); // Update the score text
    }

    // Method to reset total score when the player restarts from the first scene
    public void ResetTotalScore()
    {
        totalScore = 0;
    }

    // Method to update the score text
    public void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score\n" + currentScore.ToString();
        }
        else
        {
            Debug.LogWarning("Score text reference is not set.");
        }

        if (totalScoreText != null)
        {
            totalScoreText.text = "Total Score\n" + totalScore.ToString();
        }
        else
        {
            Debug.LogWarning("TotalScore text reference is not set.");
        }

        if (highScoreText != null)
        {
            highScoreText.text = "High Score\n" + highScore.ToString();
        }
        else
        {
            Debug.LogWarning("HighScore text reference is not set.");
        }
    }

    // Method to check and update high score
    private void CheckAndUpdateHighScore()
    {
        if (totalScore > highScore)
        {
            highScore = totalScore;
            UpdateScoreText();
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
    }

    // Method to reset the high score
    public void ResetHighScore()
    {
        highScore = 0;
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.Save();
        UpdateScoreText();
    }
}