using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurretScript : MonoBehaviour
{

    [SerializeField] private GameObject bullets;
    [SerializeField] private GameObject[] spawners;
    [SerializeField] private GameObject enemyTurretVisual;
    private PowerupSpawnerScript powerupSpawnerScript;
    private float bulletSpeed = 2.5f;
    private float rotateSpeed = 125f;
    private float rotateAmount = 0f;
    private float rotationMultiplier = 20f;
    private float hitpoints = 50f;
    private bool hasSpawnedPowerup = false;
    private float totalHitpoints = 0f;
    private float firingSpeed = 0.3f;
    private float firingStartDelay = 2f;

    private void Start()
    {
        StartCoroutine(TurretHailFire());

        powerupSpawnerScript = GameObject.FindAnyObjectByType<PowerupSpawnerScript>();

        totalHitpoints = hitpoints;
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

    public void HitByObject(int damageDone)
    {
        hitpoints -= damageDone;

        enemyTurretVisual.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, (0.25f) + ((0.75f / totalHitpoints) * hitpoints));

        foreach (Transform child in enemyTurretVisual.transform)
        {
            child.GetComponent<SpriteRenderer>().color = new Color(147f / 255f, 147f / 255f, 147f / 255f, (0.25f) + ((0.75f / totalHitpoints) * hitpoints));
        }

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
            Destroy(gameObject);
        }
    }

}