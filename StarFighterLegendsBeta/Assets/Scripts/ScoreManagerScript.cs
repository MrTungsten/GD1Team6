using UnityEngine;
using TMPro; // Add this line for TextMeshPro
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class ScoreManager : MonoBehaviour
{

    // Dictionary to store score values for different GameObject types
    private Dictionary<string, int> scoreValues;

    // Variable to hold the current score
    private int currentScore = 0;

    // Variable to hold the total score
    private static int totalScore;

    // Singleton pattern to ensure only one instance of ScoreManager exists
    public static ScoreManager Instance { get; private set; }

    private PlayerData playerData = new PlayerData();

    private void Awake()
    {

        GetScoreJson();

        // Singleton pattern
        CreateSingleton();

        // Initialize the score values for different GameObject types
        scoreValues = new Dictionary<string, int>();
        scoreValues["EnemyDiver"] = 15;
        scoreValues["EnemyPlane"] = 5;
        scoreValues["EnemyTurret"] = 20;
        scoreValues["EnemyTank"] = 10;
    }

    private void CreateSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Method to increment the score when a GameObject is destroyed
    public void IncrementScore(string gameObjectType)
    {
        if (scoreValues.ContainsKey(gameObjectType))
        {
            currentScore += scoreValues[gameObjectType];
            totalScore += scoreValues[gameObjectType];
            CheckAndUpdateHighScore(); // Update the high score
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
        totalScore += 25;
        CheckAndUpdateHighScore(); // Update the high score
    }

    // Method to check and update high score
    public void CheckAndUpdateHighScore()
    {
        if (totalScore > playerData.highScore)
        {
            playerData.highScore = totalScore;
        }
    }

    // Method to reset score when the player restarts
    public void ResetCurrentScore()
    {
        currentScore = 0;
    }

    // Method to reset total score when the player restarts from the first scene
    public void ResetTotalScore()
    {
        totalScore = 0;
    }

    // Method to update the score text
    public void UpdateScoreText(TextMeshProUGUI scoreText, TextMeshProUGUI totalScoreText, TextMeshProUGUI highScoreText)
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
            highScoreText.text = "High Score\n" + playerData.highScore.ToString();
        }
        else
        {
            Debug.LogWarning("HighScore text reference is not set.");
        }
    }

    // Method to reset total score and high score when the player restarts
    public void ResetSceneScore(int initialTotalScore, int initialHighScore)
    {
        totalScore = initialTotalScore;
        playerData.highScore = initialHighScore;
    }

    public void SetScoreJson()
    {
        string playerDataSave = JsonUtility.ToJson(playerData);
        string filePath = Application.persistentDataPath + "/PlayerScoreData.json";
        Debug.Log(filePath);
        System.IO.File.WriteAllText(filePath, playerDataSave);
        Debug.Log("Save Effective!");
        Debug.Log("High Score is " + playerData.highScore);
    }

    public void GetScoreJson()
    {
        string filePath = Application.persistentDataPath + "/PlayerScoreData.json";

        if (System.IO.File.Exists(filePath))
        {
            string playerDataSave = System.IO.File.ReadAllText(filePath);
            playerData = JsonUtility.FromJson<PlayerData>(playerDataSave);
            Debug.Log(filePath);
            Debug.Log("Load Effective!");
            Debug.Log("High Score is " + playerData.highScore);
        }
        else
        {
            SetScoreJson();
        }
    }

    // Method to reset the high score
    public void ResetHighScore()
    {
        playerData.highScore = 0;
    }

    public int GetTotalScore()
    {
        return totalScore;
    }

    public int GetHighScore()
    {
        return playerData.highScore;
    }

}

public class PlayerData
{
    public int highScore = 0;
}