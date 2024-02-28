using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{

    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject gameOverScreenWin;
    [SerializeField] private GameObject gameOverScreenLoss;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI totalScoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    private GameObject player;
    private bool isGameOver = false;
    private bool victory = false;
    private int initialTotalScore = 0;
    private int initialHighScore = 0;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (SceneUtility.GetScenePathByBuildIndex(2) == SceneManager.GetActiveScene().path)
        {
            ScoreManager.Instance.ResetTotalScore();
            ScoreManager.Instance.ResetHighScore();
            ScoreManager.Instance.SetScoreJson();

            PlayerStatsManager.Instance.ResetStats();
        }
        else
        {
            PlayerStatsManager.Instance.AddStats(2, 1, 1);
        }

        ScoreManager.Instance.ResetCurrentScore();
        ScoreManager.Instance.GetScoreJson();
        ScoreManager.Instance.UpdateScoreText(scoreText, totalScoreText, highScoreText);

        PlayerStatsManager.Instance.SetStats();

        initialTotalScore = ScoreManager.Instance.GetTotalScore();
        initialHighScore = ScoreManager.Instance.GetHighScore();

        levelText.text = string.Format("Level\n{0}/{1}", SceneManager.GetActiveScene().buildIndex - 1, SceneManager.sceneCountInBuildSettings - 3);
    }

    private void Update()
    {

        ScoreManager.Instance.UpdateScoreText(scoreText, totalScoreText, highScoreText);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemyPlane").ToArray();
        enemies = enemies.Concat(GameObject.FindGameObjectsWithTag("EnemyTank")).ToArray();
        enemies = enemies.Concat(GameObject.FindGameObjectsWithTag("EnemyTurret")).ToArray();
        enemies = enemies.Concat(GameObject.FindGameObjectsWithTag("EnemyDiver")).ToArray();

        int numOfEnemies = enemies.Length;

        if (numOfEnemies == 0 && !isGameOver)
        {
            Debug.Log("The player has won!");
            victory = true;
            isGameOver = true;
            GameOver();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            MainMenu();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            ScoreManager.Instance.ResetHighScore();
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        
        gameOverScreen.SetActive(true);

        if (victory)
        {
            GameOverWin();
        }
        else
        {
            GameOverLoss();
        }
    }

    private void GameOverWin()
    {
        gameOverScreenWin.SetActive(true);
        nextLevelButton.Select();
    }

    private void GameOverLoss()
    {
        gameOverScreenLoss.SetActive(true);
        restartButton.Select();
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public void NextLevel()
    {
        PlayerStatsManager.Instance.ChangeStats();

        ScoreManager.Instance.SetScoreJson();

        int sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        SceneManager.LoadScene(SceneUtility.GetScenePathByBuildIndex(sceneIndex));
    }

    public void Restart()
    {
        ScoreManager.Instance.ResetSceneScore(initialTotalScore, initialHighScore);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(SceneUtility.GetScenePathByBuildIndex(0));
    }

}