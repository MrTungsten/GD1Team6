using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{

    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject gameOverScreenWin;
    [SerializeField] private GameObject gameOverScreenLoss;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI totalScoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI countdownText;

    private bool isGameOver = false;
    private bool victory = false;
    private int initialTotalScore = 0;
    private float escapeTimer = 0;
    private float levelTransTimer = 4f;
    private float playerLifeTime = 0f;
    private float scoreTimeLimit = 0f;
    private int sceneIndex = 0;
    private bool hasIncreasedScore = false;

    private void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex - 2;

        if (SceneUtility.GetScenePathByBuildIndex(2) == SceneManager.GetActiveScene().path)
        {
            ScoreManager.Instance.ResetTotalScore();

            PlayerStatsManager.Instance.ResetStats();
        }
        else
        {
            if ((SceneManager.GetActiveScene().buildIndex + 2) % 2 == 0)
            {
                PlayerStatsManager.Instance.AddStats(0, 1, 1);
            }
        }

        if (sceneIndex <= 5)
        {
            scoreTimeLimit = 60f;
        }
        else
        {
            scoreTimeLimit = 120f;
        }

        ScoreManager.Instance.ResetCurrentScore();
        ScoreManager.Instance.UpdateScoreText(scoreText, totalScoreText, highScoreText);

        PlayerStatsManager.Instance.SetStats();

        initialTotalScore = ScoreManager.Instance.GetTotalScore();

        levelText.text = string.Format("Level\n{0}/{1}", SceneManager.GetActiveScene().buildIndex - 1, SceneManager.sceneCountInBuildSettings - 3);
    }

    private void Update()
    {

        playerLifeTime += Time.deltaTime;

        ScoreManager.Instance.UpdateScoreText(scoreText, totalScoreText, highScoreText);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemyPlane").ToArray();
        enemies = enemies.Concat(GameObject.FindGameObjectsWithTag("EnemyTank")).ToArray();
        enemies = enemies.Concat(GameObject.FindGameObjectsWithTag("EnemyTurret")).ToArray();
        enemies = enemies.Concat(GameObject.FindGameObjectsWithTag("EnemyDiver")).ToArray();
        enemies = enemies.Concat(GameObject.FindGameObjectsWithTag("EnemySine")).ToArray();

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
            if (escapeTimer > 3f)
            {
                MainMenu();
            }
            else
            {
                escapeTimer += Time.deltaTime;
            }
        }
        else
        {
            escapeTimer = 0f;
        }

        if (isGameOver)
        {

            if (!hasIncreasedScore)
            {
                if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
                {
                    ScoreManager.Instance.IncrementScore(250);
                    ScoreManager.Instance.UpdateScoreText(scoreText, totalScoreText, highScoreText);
                }

                if (playerLifeTime < scoreTimeLimit)
                {
                    ScoreManager.Instance.IncrementScore((int)((scoreTimeLimit - playerLifeTime) / 5) * 5);
                    ScoreManager.Instance.UpdateScoreText(scoreText, totalScoreText, highScoreText);
                }

                hasIncreasedScore = true;
            }

            if (levelTransTimer <= 0 && victory)
            {
                NextLevel();
            }
            else if (levelTransTimer <= 0 && !victory)
            {
                SceneManager.LoadScene(SceneUtility.GetScenePathByBuildIndex(SceneManager.sceneCountInBuildSettings - 1));
            }
            else
            {
                if (levelTransTimer >= 3)
                    countdownText.text = "Continuing in 3";
                else if (levelTransTimer >= 2)
                    countdownText.text = "Continuing in 2";
                else if (levelTransTimer >= 1)
                    countdownText.text = "Continuing in 1";
                else
                    countdownText.text = "Continuing in 0";

                levelTransTimer -= Time.deltaTime;
            }
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
    }
    
    private void GameOverLoss()
    {
        gameOverScreenLoss.SetActive(true);
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public void NextLevel()
    {
        PlayerStatsManager.Instance.ChangeStats();

        int sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        SceneManager.LoadScene(SceneUtility.GetScenePathByBuildIndex(sceneIndex));
    }

    public void Restart()
    {
        ScoreManager.Instance.ResetSceneScore(initialTotalScore);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(SceneUtility.GetScenePathByBuildIndex(0));
    }

}