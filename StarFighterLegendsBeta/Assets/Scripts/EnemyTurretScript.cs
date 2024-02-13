using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurretScript : MonoBehaviour
{

    [SerializeField] private GameObject bullets;
    [SerializeField] private GameObject[] spawners;
    [SerializeField] private GameObject enemyTurretVisual;
    private PowerupSpawnerScript powerupSpawnerScript;
    private float bulletSpeed = 5f;
    private float rotateSpeed = 100f;
    private float rotateAmount = 0f;
    private float rotationMultiplier = 20f;
    private float hitpoints = 50f;
    private bool hasSpawnedPowerup = false;
    private float totalHitpoints = 0f;

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
        yield return new WaitForSeconds(3f);
        while (true)
        {
            for (int i = 0; i < 6; i++)
            {
                GameObject spawnedBullet = Instantiate(bullets, spawners[i].transform.position, spawners[i].transform.rotation);
                spawnedBullet.GetComponent<EnemyBulletScript>().SetSpeed(bulletSpeed);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    public void HitByObject(int damageDone)
    {
        hitpoints -= damageDone;

        enemyTurretVisual.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, (0.25f) + ((0.75f / totalHitpoints) * hitpoints));


        if (hitpoints <= 0 && !hasSpawnedPowerup)
        {
            hasSpawnedPowerup = true;
            powerupSpawnerScript.GetComponent<PowerupSpawnerScript>().SpawnPowerup(transform, "Laser");
            ScoreManager.Instance.IncrementScore(gameObject.tag);
            Destroy(gameObject);
        }
    }

}