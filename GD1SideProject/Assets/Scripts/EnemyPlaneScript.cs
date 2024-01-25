using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlaneScript : MonoBehaviour
{

    [SerializeField] private GameObject planeAimer;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private bool autoMove = true;
    private GameManagerScript gameManagerScript;
    private PathingScript pathingScript;
    private GameObject player;
    private float hitpoints = 5f;
    private float xBoundary = 6.35f;
    private float moveDir = 1f;
    private float speed = 5f;
    private float timer = 0f;
    private float bulletCooldown = 1f;
    private float bulletSpeed = 7.5f;

    private void Start()
    {
        if (Random.Range(0, 2) == 0)
            moveDir = 1f;
        else
            moveDir = -1f;

        gameManagerScript = GameObject.FindAnyObjectByType<GameManagerScript>();

        pathingScript = this.GetComponent<PathingScript>();
        pathingScript.SetPathingSpeed(speed);
        pathingScript.SetRotationSpeed(0);
    }

    private void Update()
    {

        if (autoMove)
        {
            MoveBackAndForth();
        }
        
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
        planeAimer.transform.rotation = Quaternion.Euler(0, 0, angle + 90);

        if (timer > bulletCooldown)
        {
            GameObject spawnedBullet = Instantiate(bulletPrefab, planeAimer.transform.position + new Vector3(-0.5f, 0, 0), planeAimer.transform.rotation);
            spawnedBullet.GetComponent<EnemyBulletScript>().SetSpeed(bulletSpeed);
            bulletCooldown = Random.Range(2f, 4f);
            timer = 0f;
        }
        else
        {
            timer += Time.deltaTime;
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
