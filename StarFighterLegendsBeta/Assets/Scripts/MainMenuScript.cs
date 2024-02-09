using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

    [SerializeField] private GameObject title;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject backgroundImage;

    private void Start()
    {
        title.SetActive(true);
        mainMenu.SetActive(true);
        backgroundImage.SetActive(true);
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