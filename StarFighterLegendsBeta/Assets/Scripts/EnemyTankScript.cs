using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTankScript : MonoBehaviour
{

    [SerializeField] private GameObject tankAimer;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject enemyTankVisual;
    private PowerupSpawnerScript powerupSpawnerScript;
    private GameManagerScript gameManagerScript;
    private PathingScript pathingScript;
    private GameObject player;
    private float hitpoints = 7f;
    private float totalHitpoints = 0f;
    private float timer = 0f;
    private float bulletCooldown = 3f;
    private float bulletSpeed = 10f;
    private float speed = 2.5f;
    private float rotationSpeed = 125f;
    private bool hasSpawnedPowerup = false;
    

    private void Start()
    {
        gameManagerScript = GameObject.FindAnyObjectByType<GameManagerScript>();

        pathingScript = this.GetComponent<PathingScript>();
        if (pathingScript != null)
        {
            pathingScript.SetPathingSpeed(speed);
            pathingScript.SetRotationSpeed(rotationSpeed);
        }

        powerupSpawnerScript = GameObject.FindAnyObjectByType<PowerupSpawnerScript>();

        totalHitpoints = hitpoints;
    }

    private void Update()
    {
        if (!gameManagerScript.IsGameOver())
        {
            FireAtPlayer();
        }
    }

    public void HitByObject(int damageDone)
    {
        hitpoints -= damageDone;

        enemyTankVisual.GetComponent<SpriteRenderer>().color = new Color(0.84f, 0f, 1f, (0.75f) + ((0.25f / totalHitpoints) * hitpoints));

        if (hitpoints <= 0 && !hasSpawnedPowerup)
        {
            hasSpawnedPowerup = true;

            int randomNum = Random.Range(1, 7);

            if (randomNum <= 4)
            {
                powerupSpawnerScript.GetComponent<PowerupSpawnerScript>().SpawnPowerup(transform, "Bomb");
            }
            else if (randomNum >= 5)
            {
                powerupSpawnerScript.GetComponent<PowerupSpawnerScript>().SpawnPowerup(transform, "Score");
            }
            else
            {
                powerupSpawnerScript.GetComponent<PowerupSpawnerScript>().SpawnPowerup(transform, "Laser");
            }

            ScoreManager.Instance.IncrementScore(gameObject.tag);

            Destroy(gameObject);
        }
    }
    private void FireAtPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 dir = player.transform.position - transform.position;
        float angle = Mathf.Atan2(-dir.y, -dir.x) * Mathf.Rad2Deg;
        tankAimer.transform.rotation = Quaternion.Euler(0, 0, angle + 90);

        if (timer > bulletCooldown)
        {
            GameObject spawnedBullet = Instantiate(bulletPrefab, tankAimer.transform.position + new Vector3(-0.5f, 0, 0), tankAimer.transform.rotation);
            spawnedBullet.GetComponent<EnemyBulletScript>().SetSpeed(bulletSpeed);
            bulletCooldown = Random.Range(2f, 4f);
            timer = 0f;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

}