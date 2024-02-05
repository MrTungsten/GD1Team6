using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawnerScript : MonoBehaviour
{

    [SerializeField] private GameObject bombPowerupPrefab;
    [SerializeField] private GameObject laserPowerupPrefab;

    public void SpawnPowerup(Transform location, string powerupName)
    {
        if (powerupName == "Bomb")
        {
            Instantiate(bombPowerupPrefab, location.position, location.rotation);
        }
        else if (powerupName == "Laser")
        {
            Instantiate(laserPowerupPrefab, location.position, location.rotation);
        }
    }

}