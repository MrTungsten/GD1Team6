using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlaneScript : MonoBehaviour
{
    public enum ShotType
    {
        normal,
        triple,
        cross
    }

    [SerializeField] private GameObject planeAimer1;
    [SerializeField] private GameObject planeAimer2;
    [SerializeField] private GameObject planeAimer3;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private bool autoMove = true;
    [SerializeField] private ShotType shotType = ShotType.normal;
    [SerializeField] private float speed = 3f;
    [SerializeField] private GameObject enemyPlaneVisual;
    [SerializeField] private GameObject deathExplosion;
    private PowerupSpawnerScript powerupSpawnerScript;
    private GameManagerScript gameManagerScript;
    private PathingScript pathingScript;
    private GameObject player;
    private float xBoundary = 9.25f;
    private float moveDir = 1f;
    private float timer = 0f;
    private float bulletCooldown = 1f;
    private float bulletSpeed = 4f;
    private bool hasSpawnedPowerup = false;
    private float hitpoints = 5f;

    private void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");

        if (Random.Range(0, 2) == 0)
            moveDir = 1f;
        else
            moveDir = -1f;

        gameManagerScript = GameObject.FindAnyObjectByType<GameManagerScript>();
        powerupSpawnerScript = GameObject.FindAnyObjectByType<PowerupSpawnerScript>();

        pathingScript = this.GetComponent<PathingScript>();
        if (pathingScript != null)
        {
            pathingScript.SetPathingSpeed(speed);
            pathingScript.SetRotationSpeed(0);
        }

        switch (shotType)
        {
            case ShotType.normal:
                hitpoints = 5f;
                break;

            case ShotType.triple:
                hitpoints = 10f;
                break;

            case ShotType.cross:
                hitpoints = 15f;
                break;
        }

    }

    private void Update()
    {

        if (autoMove)
        {
            MoveBackAndForth();
        }
        
        if (!gameManagerScript.IsGameOver())
        {
            FireAtPlayer();
        }

    }

    public void HitByObject(float damageDone)
    {
        hitpoints -= damageDone;

        StartCoroutine(HitEffect());

        if (hitpoints <= 0 && !hasSpawnedPowerup)
        {
            hasSpawnedPowerup = true;

            int randomNum = Random.Range(1, 101);

            if (randomNum <= 10)
            {
                powerupSpawnerScript.GetComponent<PowerupSpawnerScript>().SpawnPowerup(transform, "Bomb");
            }
            else if (randomNum <= 15)
            {
                powerupSpawnerScript.GetComponent<PowerupSpawnerScript>().SpawnPowerup(transform, "Score");
            }

            ScoreManagerScript.Instance.IncrementScore(gameObject.tag);

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

    private void FireAtPlayer()
    {
        if (shotType == ShotType.normal)
        {

            Vector3 dir = player.transform.position - transform.position;
            float angle = Mathf.Atan2(-dir.y, -dir.x) * Mathf.Rad2Deg;
            planeAimer2.transform.rotation = Quaternion.Euler(0, 0, angle + 90);
            bulletCooldown = Random.Range(1f, 2f);

            if (timer > bulletCooldown)
            {
                GameObject spawnedBullet = Instantiate(bulletPrefab, planeAimer2.transform.position, planeAimer2.transform.rotation);
                spawnedBullet.GetComponent<EnemyBulletScript>().SetSpeed(bulletSpeed);
                timer = 0f;
                bulletCooldown = Random.Range(1f, 2f);
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
        else if (shotType == ShotType.triple)
        {

            planeAimer2.transform.rotation = Quaternion.Euler(0, 0, 180f);
            planeAimer1.transform.rotation = Quaternion.Euler(0, 0, 170f);
            planeAimer3.transform.rotation = Quaternion.Euler(0, 0, 190f);

            bulletCooldown = Random.Range(3f, 6f);

            if (timer > bulletCooldown)
            {
                StartCoroutine(TripleShotBurst());
                timer = 0f;
                bulletCooldown = Random.Range(3f, 6f);
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
        else if (shotType == ShotType.cross)
        {

            bulletCooldown = Random.Range(5f, 10f);

            if (timer > bulletCooldown)
            {
                StartCoroutine(CrossShotBurst());
                timer = 0f;
                bulletCooldown = Random.Range(5f, 10f);
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }

    private IEnumerator TripleShotBurst()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject spawnedBullet1 = Instantiate(bulletPrefab, planeAimer1.transform.position, planeAimer1.transform.rotation);
            GameObject spawnedBullet2 = Instantiate(bulletPrefab, planeAimer2.transform.position, planeAimer2.transform.rotation);
            GameObject spawnedBullet3 = Instantiate(bulletPrefab, planeAimer3.transform.position, planeAimer3.transform.rotation);
            spawnedBullet1.GetComponent<EnemyBulletScript>().SetSpeed(bulletSpeed);
            spawnedBullet2.GetComponent<EnemyBulletScript>().SetSpeed(bulletSpeed);
            spawnedBullet3.GetComponent<EnemyBulletScript>().SetSpeed(bulletSpeed);
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator CrossShotBurst()
    {
        Vector3 dir = player.transform.position - transform.position;
        for (int i = 0; i < 5; i++)
        {   
            float angle = Mathf.Atan2(-dir.y, -dir.x) * Mathf.Rad2Deg;
            planeAimer1.transform.rotation = Quaternion.Euler(0, 0, angle + 100f);
            planeAimer3.transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
            GameObject spawnedBullet1 = Instantiate(bulletPrefab, planeAimer1.transform.position, planeAimer1.transform.rotation);
            GameObject spawnedBullet3 = Instantiate(bulletPrefab, planeAimer3.transform.position, planeAimer3.transform.rotation);
            spawnedBullet1.GetComponent<EnemyBulletScript>().SetSpeed(bulletSpeed);
            spawnedBullet3.GetComponent<EnemyBulletScript>().SetSpeed(bulletSpeed);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void MoveBackAndForth()
    {
        if (transform.position.x > xBoundary)
        {
            moveDir = -1f;
        }
        else if (transform.position.x < -xBoundary)
        {
            moveDir = 1f;
        }

        transform.position += new Vector3(moveDir, 0, 0) * speed * Time.deltaTime;
    }

}