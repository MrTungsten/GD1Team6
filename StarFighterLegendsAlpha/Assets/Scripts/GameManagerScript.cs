using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{

    private bool isGameOver = false;

    private void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemyPlane").ToArray();
        enemies = enemies.Concat(GameObject.FindGameObjectsWithTag("EnemyTank")).ToArray();
        enemies = enemies.Concat(GameObject.FindGameObjectsWithTag("EnemyTurret")).ToArray();
        enemies = enemies.Concat(GameObject.FindGameObjectsWithTag("EnemyDiver")).ToArray();

        int numOfEnemies = enemies.Length;

        if (numOfEnemies == 0 && !isGameOver)
        {
            Debug.Log("The player has won!");
            isGameOver = true;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown (KeyCode.Alpha1))
        {
            int sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (sceneIndex > 4)
            {
                sceneIndex = 0;
            }
            SceneManager.LoadScene(SceneUtility.GetScenePathByBuildIndex(sceneIndex));
        }
    }

    public void GameOver()
    {
        isGameOver = true;
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

}
