using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyDiverPlaneScript : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private GameObject enemyDiverVisual;
    [SerializeField] private GameObject deathExplosion;
    private GameObject player;
    private GameManagerScript gameManagerScript;
    private PowerupSpawnerScript powerupSpawnerScript;
    private bool isDashing = false;
    private float dashingPower = 70f;
    private float dashTime = 1.5f;
    private float returnSpeed = 5f;
    private float dashingCooldown = 5f;
    private float timer = 0f;
    private int waypointCount = -1;
    private bool isReturning = false;
    private int hitpoints = 15;
    private bool hasSpawnedPowerup = false;

    private void Start()
    {

        gameManagerScript = GameObject.FindAnyObjectByType<GameManagerScript>();

        powerupSpawnerScript = GameObject.FindAnyObjectByType<PowerupSpawnerScript>();

        player = GameObject.FindGameObjectWithTag("Player");

        dashingCooldown = Random.Range(4, 7);
        
    }

    private void Update()
    {

        if (!isDashing && !isReturning && !gameManagerScript.IsGameOver())
        {
            if (timer > dashingCooldown)
            {
                StartCoroutine(Dash());
                timer = 0f;
                dashingCooldown = Random.Range(4, 7);
            }
            else
            {
                Vector3 direction = player.transform.position - transform.position;
                float angle = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle + 90);
                timer += Time.deltaTime;
            }
        }

        if (isReturning)
        {
            Vector3 direction = waypoints[waypointCount].transform.position - transform.position;
            float angle = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle + 90);

            if (transform.position != waypoints[waypointCount].transform.position)
            {
                transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointCount].transform.position, returnSpeed * Time.deltaTime);
            }
            else
            {
                isReturning = false;
            }
        }

        if (gameManagerScript.IsGameOver())
        {
            StopAllCoroutines();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }

    private IEnumerator Dash()
    {

        isDashing = true;

        Vector3 direction = player.transform.position - transform.position;
        float angle = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);

        rb.AddForce(direction * dashingPower);

        yield return new WaitForSeconds(dashTime);

        rb.velocity = Vector3.zero;

        
        if (waypointCount == (waypoints.Length - 1))
        {
            waypointCount = 0;
        }
        else
        {
            waypointCount++;
        }

        isDashing = false;
        isReturning = true;

    }

    public void HitByObject(int damageDone)
    {
        hitpoints -= damageDone;

        StartCoroutine(HitEffect());

        if (hitpoints <= 0 && !hasSpawnedPowerup)
        {
            hasSpawnedPowerup = true;

            int randomNum = Random.Range(1, 101);

            if (randomNum <= 10)
            {
                powerupSpawnerScript.GetComponent<PowerupSpawnerScript>().SpawnPowerup(transform, "Laser");
            }
            else if (randomNum <= 30)
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
        enemyDiverVisual.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        enemyDiverVisual.GetComponent<SpriteRenderer>().color = Color.white;
    }

}