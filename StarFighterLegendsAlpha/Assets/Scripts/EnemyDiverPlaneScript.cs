using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyDiverPlaneScript : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject waypoint1;
    [SerializeField] private GameObject waypoint2;
    [SerializeField] private GameObject waypoint3;
    private GameObject player;
    private GameObject[] waypoints = new GameObject[3];
    private GameManagerScript gameManagerScript;
    private PowerupSpawnerScript powerupSpawnerScript;
    private bool isDashing = false;
    private float dashingPower = 60f;
    private float dashTime = 1.5f;
    private float returnSpeed = 5f;
    private float dashingCooldown = 5f;
    private float timer = 0f;
    private int waypointCount = 1;
    private bool isReturning = false;
    private int hitpoints = 10;

    private void Start()
    {

        gameManagerScript = GameObject.FindAnyObjectByType<GameManagerScript>();

        powerupSpawnerScript = GameObject.FindAnyObjectByType<PowerupSpawnerScript>();

        player = GameObject.FindGameObjectWithTag("Player");
        waypoints[0] = waypoint1;
        waypoints[1] = waypoint2;
        waypoints[2] = waypoint3;

    }

    private void Update()
    {

        if (!isDashing && !isReturning && !gameManagerScript.IsGameOver())
        {
            if (timer > dashingCooldown)
            {
                StartCoroutine(Dash());
                timer = 0f;
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

        
        if (waypointCount == 2)
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

        if (hitpoints <= 0)
        {
            powerupSpawnerScript.GetComponent<PowerupSpawnerScript>().SpawnPowerup(transform, "Laser");
            Destroy(gameObject);
        }
    }

}