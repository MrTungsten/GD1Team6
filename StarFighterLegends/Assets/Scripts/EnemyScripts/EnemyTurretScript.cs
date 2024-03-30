using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurretScript : MonoBehaviour
{

    [SerializeField] private GameObject bullets;
    [SerializeField] private GameObject[] spawners;
    [SerializeField] private GameObject enemyTurretVisual;
    [SerializeField] private GameObject deathExplosion;
    private PowerupSpawnerScript powerupSpawnerScript;
    private float bulletSpeed = 2.5f;
    private float rotateSpeed = 125f;
    private float rotateAmount = 0f;
    private float rotationMultiplier = 20f;
    private float hitpoints = 75f;
    private bool hasSpawnedPowerup = false;
    private float firingSpeed = 0.3f;
    private float firingStartDelay = 2f;

    private void Start()
    {
        StartCoroutine(TurretHailFire());

        powerupSpawnerScript = GameObject.FindAnyObjectByType<PowerupSpawnerScript>();
    }

    private void Update()
    {
        rotateAmount += rotationMultiplier * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 10 * rotateAmount), rotateSpeed * Time.deltaTime);
    }

    private IEnumerator TurretHailFire()
    {
        yield return new WaitForSeconds(firingStartDelay);

        while (true)
        {
            for (int i = 0; i < 6; i++)
            {
                GameObject spawnedBullet = Instantiate(bullets, spawners[i].transform.position, spawners[i].transform.rotation);
                spawnedBullet.GetComponent<EnemyBulletScript>().SetSpeed(bulletSpeed);
            }

            yield return new WaitForSeconds(firingSpeed);
        }
    }

    private IEnumerator HitEffect()
    {
        enemyTurretVisual.GetComponent<SpriteRenderer>().color = Color.red;
        foreach (Transform child in enemyTurretVisual.transform)
        {
            child.GetComponent<SpriteRenderer>().color = Color.red;
        }

        yield return new WaitForSeconds(0.1f);

        enemyTurretVisual.GetComponent<SpriteRenderer>().color = new Color(0f, 208f / 255f, 255f, 1f);
        foreach (Transform child in enemyTurretVisual.transform)
        {
            child.GetComponent<SpriteRenderer>().color = new Color(147f / 255f, 147f / 255f, 147f / 255f, 1f);
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

            if (randomNum <= 20)
            {
                powerupSpawnerScript.GetComponent<PowerupSpawnerScript>().SpawnPowerup(transform, "Laser");
            }
            else
            {
                powerupSpawnerScript.GetComponent<PowerupSpawnerScript>().SpawnPowerup(transform, "Score");
            }

            ScoreManager.Instance.IncrementScore(gameObject.tag);

            GameObject explosion = Instantiate(deathExplosion, transform.position, transform.rotation);
            explosion.transform.localScale = new Vector3(2f, 2f, 1);

            Destroy(gameObject);
        }
    }

}