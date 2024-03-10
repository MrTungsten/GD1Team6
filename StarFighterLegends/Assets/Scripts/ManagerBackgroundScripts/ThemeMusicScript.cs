using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeMusicScript : MonoBehaviour
{

    private static ThemeMusicScript Instance;

    private void Awake()
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

}
