using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionScreenScript : MonoBehaviour
{

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(SceneUtility.GetScenePathByBuildIndex(2));
        }
    }

}
