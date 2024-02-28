using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject playerBulletSpawner;
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private GameObject laser;
    [SerializeField] private AudioClip laserFire;
    [SerializeField] private TextMeshProUGUI bombCountText;
    [SerializeField] private TextMeshProUGUI laserCountText;
    [SerializeField] private TextMeshProUGUI hitpointCountText;
    private GameManagerScript gameManagerScript;
    private Collider2D laserCollider = null;
    private float moveSpeed = 8f;
    private float xBoundary = 6.35f;
    private float yBoundary = 6.7f;
    private float bulletTimer = 0f;
    private float bulletCooldown = .25f;
    private float bombTimer = 3f;
    private float bombCooldown = 0.5f;
    private float laserTimer = 7f;
    private float laserCooldown = 7f;
    private float hitpoints;
    private bool hasImmunity = false;
    private int bombCount;
    private int laserCount;
    private bool isLaserOn = false;
    private float laserLifeTime = 4f;
    private float laserDamageTimer = 0.5f;
    private float laserDamageCooldown = 0.5f;

    private void Start()
    {
        gameManagerScript = FindAnyObjectByType<GameManagerScript>();

        laserCollider = laser.GetComponent<Collider2D>();
        laser.gameObject.SetActive(false);

        bulletTimer = bulletCooldown;
    }

    private void Update()
    {

        Vector2 inputVector = new Vector2 (0, 0);

        if (Input.GetKey(KeyCode.UpArrow))
        {
            inputVector.y = 1f;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            inputVector.y = -1f;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            inputVector.x = -1f;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            inputVector.x = 1f;
        }

        inputVector = inputVector.normalized;

        Vector3 moveDir = new Vector3(inputVector.x, inputVector.y, 0);
        transform.position += moveDir * Time.deltaTime * moveSpeed;

        if (transform.position.x > xBoundary)
        {
            transform.position = new Vector3(xBoundary, transform.position.y, 0);
        }
        else if (transform.position.x < -xBoundary)
        {
            transform.position = new Vector3(-xBoundary, transform.position.y, 0);
        }

        if (transform.position.y > yBoundary)
        {
            transform.position = new Vector3(transform.position.x, yBoundary, 0);
        }
        else if (transform.position.y < -yBoundary)
        {
            transform.position = new Vector3(transform.position.x, -yBoundary, 0);
        }

        if (bulletTimer >= bulletCooldown)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                Instantiate(bulletPrefab, playerBulletSpawner.transform.position + new Vector3(-0.15f, 0, 0), transform.rotation);
                Instantiate(bulletPrefab, playerBulletSpawner.transform.position + new Vector3(0.15f, 0, 0), transform.rotation);
                bulletTimer = 0f;
                AudioSource.PlayClipAtPoint(laserFire, playerBulletSpawner.transform.position);
            } 
        }
        else
        {
            bulletTimer += Time.deltaTime;
        }

        if (bombTimer >= bombCooldown)
        {
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                if (bombCount > 0)
                {
                    Instantiate(bombPrefab, transform.position, transform.rotation);
                    bombCount--;
                    UpdateStats();
                    bombTimer = 0f;
                }
            }
        }
        else
        {
            bombTimer += Time.deltaTime;
        }

        if (laserTimer >= laserCooldown)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (laserCount > 0 && isLaserOn != true)
                {
                    StartCoroutine(Laser());
                    laserCount--;
                    UpdateStats();
                    laserTimer = 0f;
                }
            }
        }
        else
        {
            laserTimer += Time.deltaTime;
        }

        if (isLaserOn)
        {

            List<Collider2D> results = new List<Collider2D>();
            int numOfCollisions = Physics2D.OverlapCollider(laserCollider, new ContactFilter2D().NoFilter(), results);

            for (int i = 0; i < numOfCollisions; i++)
            {
                if (results[i].gameObject.layer == 7)
                {
                    if (laserDamageTimer >= laserDamageCooldown)
                    {
                        if (results[i].gameObject.CompareTag("EnemyPlane"))
                        {
                            results[i].gameObject.GetComponent<EnemyPlaneScript>().HitByObject(2);
                        }
                        else if (results[i].gameObject.CompareTag("EnemyTank"))
                        {
                            results[i].gameObject.GetComponent<EnemyTankScript>().HitByObject(3);
                        }
                        else if (results[i].gameObject.CompareTag("EnemyTurret"))
                        {
                            results[i].gameObject.GetComponent<EnemyTurretScript>().HitByObject(4);
                        }
                        else if (results[i].gameObject.CompareTag("EnemyDiver"))
                        {
                            results[i].gameObject.GetComponent<EnemyDiverPlaneScript>().HitByObject(5);
                        }
                        laserDamageTimer = 0f;
                    }
                    else
                    {
                        laserDamageTimer += Time.deltaTime;
                    }
                }
                else
                {
                    if (!results[i].gameObject.CompareTag("Powerup") && !results[i].gameObject.CompareTag("Bomb"))
                    {
                        Destroy(results[i].gameObject);
                    }
                }
            }
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HitByObject(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        HitByObject(collision);
    }

    private void HitByObject(Collider2D collision)
    {
        if (!hasImmunity && !collision.gameObject.CompareTag("Powerup") && !collision.gameObject.CompareTag("Bomb") && !gameManagerScript.IsGameOver())
        {
            hitpoints--;

            UpdateStats();

            hitpointCountText.text = hitpoints + "x";

            if (hitpoints <= 0)
            {
                Debug.Log("Player has lost!");
                gameManagerScript.GameOver();
                Destroy(gameObject);
            }

            StartCoroutine(HitImmunity(0.5f));
        }
    }

    public IEnumerator HitImmunity(float immuneTime)
    {
        hasImmunity = true;
        yield return new WaitForSeconds(immuneTime);
        hasImmunity = false;
    }

    public void GainedPowerup(string powerupName)
    {
        if (powerupName == "Bomb")
        {
            bombCount++;
        }
        if (powerupName == "Laser")
        {
            laserCount++;
        }
        if (powerupName == "Score")
        {
            ScoreManager.Instance.ScorePowerup();
        }

        UpdateStats();
    }

    private IEnumerator Laser()
    {
        isLaserOn = true;
        laser.gameObject.SetActive(true);
        laserCollider.enabled = true;
        yield return new WaitForSeconds(laserLifeTime);
        isLaserOn = false;
        laser.gameObject.SetActive(false);
        laserCollider.enabled = false;
    }

    public void SetStats(int _health, int _bombCount, int _laserCount)
    {
        hitpoints = _health;
        bombCount = _bombCount;
        laserCount = _laserCount;
        UpdateStats();
    }

    private void UpdateStats()
    {
        hitpointCountText.text = hitpoints + "x";
        bombCountText.text = bombCount + "x";
        laserCountText.text = laserCount + "x";
    }

    public int[] GetStats()
    {
        return new int[] { (int)hitpoints, bombCount, laserCount };
    }

}