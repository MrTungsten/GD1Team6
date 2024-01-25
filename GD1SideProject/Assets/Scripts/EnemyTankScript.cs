using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTankScript : MonoBehaviour
{

    [SerializeField] private GameObject tankAimer;
    [SerializeField] private GameObject bulletPrefab;
    private GameManagerScript gameManagerScript;
    private PathingScript pathingScript;
    private GameObject player;
    private float hitpoints = 7f;
    private float timer = 0f;
    private float bulletCooldown = 3f;
    private float bulletSpeed = 10f;
    private float speed = 2.5f;
    private float rotationSpeed = 125f;

    private void Start()
    {
        gameManagerScript = GameObject.FindAnyObjectByType<GameManagerScript>();
        
        pathingScript = this.GetComponent<PathingScript>();
        pathingScript.SetPathingSpeed(speed);
        pathingScript.SetRotationSpeed(rotationSpeed);
    }

    private void Update()
    {
        if (hitpoints <= 0f)
        {
            Destroy(gameObject);
        }

        if (!gameManagerScript.IsGameOver())
        {
            FireAtPlayer();
        }
    }

    public void HitByObject(string objectName)
    {
        if (objectName == "Bullet")
        {
            hitpoints--;
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