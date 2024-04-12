using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySinePlaneScript : MonoBehaviour
{

    [SerializeField] private GameObject planeAimer1;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject enemyPlaneVisual;
    [SerializeField] private GameObject deathExplosion;
    private GameManagerScript gameManagerScript;
    private PowerupSpawnerScript powerupSpawnerScript;
    private GameObject player;
    private float timer = 0f;
    private float bulletCooldown = 4f;
    private float bulletSpeed = 4f;
    private bool isShooting = false;
    private int amountOfBullets = 35;
    private bool hasSpawnedPowerup = false;
    private float hitpoints = 20f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameManagerScript = GameObject.FindAnyObjectByType<GameManagerScript>();
        powerupSpawnerScript = GameObject.FindAnyObjectByType<PowerupSpawnerScript>();

        timer = bulletCooldown;
    }

    private void Update()
    {
        if (!gameManagerScript.IsGameOver())
        {
            FireAtPlayer();
        }
    }

    private void FireAtPlayer()
    {
        if (!isShooting)
        {
            if (timer > bulletCooldown)
            {
                Vector3 dir = player.transform.position - transform.position;
                float angle = Mathf.Atan2(-dir.y, -dir.x) * Mathf.Rad2Deg;
                planeAimer1.transform.rotation = Quaternion.Euler(0, 0, angle + 90);

                StartCoroutine(ShootHelix());

                timer = 0;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }

    private IEnumerator ShootHelix()
    {
        isShooting = true;

        for (int i = 0; i < amountOfBullets; i++)
        {
            GameObject spawnedBullet1 = Instantiate(bulletPrefab, planeAimer1.transform.position, planeAimer1.transform.rotation);
            GameObject spawnedBullet2 = Instantiate(bulletPrefab, planeAimer1.transform.position, planeAimer1.transform.rotation);
            spawnedBullet1.GetComponent<SinusoidalPath>().enabled = true;
            spawnedBullet1.GetComponent<EnemyBulletScript>().SetAutoMove(false);
            spawnedBullet1.GetComponent<SinusoidalPath>().SetSettings(1, bulletSpeed, 4.5f, 1f);
            spawnedBullet2.GetComponent<SinusoidalPath>().enabled = true;
            spawnedBullet2.GetComponent<EnemyBulletScript>().SetAutoMove(false);
            spawnedBullet2.GetComponent<SinusoidalPath>().SetSettings(-1, bulletSpeed, 4.5f, 1f);
            yield return new WaitForSeconds(0.06f);
        }

        isShooting = false;
    }

    public void HitByObject(float damageDone)
    {
        hitpoints -= damageDone;

        StartCoroutine(HitEffect());

        if (hitpoints <= 0 && !hasSpawnedPowerup)
        {
            hasSpawnedPowerup = true;

            int randomNum = Random.Range(1, 101);

            if (randomNum <= 20)
            {
                powerupSpawnerScript.GetComponent<PowerupSpawnerScript>().SpawnPowerup(transform, "Laser");
            }
            else if (randomNum <= 50)
            {
                powerupSpawnerScript.GetComponent<PowerupSpawnerScript>().SpawnPowerup(transform, "Score");
            }

            ScoreManager.Instance.IncrementScore(gameObject.tag);

            Instantiate(deathExplosion, transform.position, transform.rotation);

            Destroy(gameObject);
        }
    }

    private IEnumerator HitEffect()
    {
        enemyPlaneVisual.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        enemyPlaneVisual.GetComponent<SpriteRenderer>().color = Color.white;
    }

}