using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{

    [SerializeField] private GameObject title;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject backgroundImage;
    [SerializeField] private Button playButton;
    private float escapeTimer = 0f;

    private void Start()
    {
        title.SetActive(true);
        mainMenu.SetActive(true);
        backgroundImage.SetActive(true);
        playButton.Select();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (escapeTimer > 6f)
            {
                Application.Quit();
                Debug.Log("Quit Game");
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
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1));
        title.SetActive(false);
        mainMenu.SetActive(false);
        backgroundImage.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}