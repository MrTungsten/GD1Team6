using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

    [SerializeField] private GameObject title;
    [SerializeField] private GameObject mainMenu;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1));
        Destroy(title);
        Destroy(mainMenu);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}