using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{

    private GameObject player;
    private static int[] playerStats = null;
    private int[] defaultStats = new int[] { 7, 1, 1 };

    public static PlayerStatsManager Instance { get; private set; }

    private void Awake()
    {
        CreateSingleton();

        player = GameObject.FindGameObjectWithTag("Player");
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

    public void AddStats(int healthToAdd, int bombCountToAdd, int laserCountToAdd)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = new int[] { playerStats[0] + healthToAdd, playerStats[1] + bombCountToAdd, playerStats[2] + laserCountToAdd };
        SetStats();
        Debug.Log("Added Stats!");
    }

    public void ChangeStats()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerScript>().GetStats();
    }

    public void SetStats()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerScript>().SetStats(playerStats[0], playerStats[1], playerStats[2]);
    }

    public void ResetStats()
    {
        playerStats = defaultStats;
    }

}
