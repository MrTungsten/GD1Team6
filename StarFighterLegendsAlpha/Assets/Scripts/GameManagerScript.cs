using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{

    private bool isGameOver = false;

    private void Update()
    {
        if (GameObject.FindGameObjectWithTag("EnemyPlane") == null && GameObject.FindGameObjectWithTag("EnemyTank") == null && GameObject.FindGameObjectWithTag("EnemyTurret") == null && GameObject.FindGameObjectWithTag("EnemyDiver") == null && !isGameOver)
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
