using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject playerBulletSpawner;
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private GameObject laser;
    [SerializeField] private TextMeshProUGUI bombCountText;
    [SerializeField] private TextMeshProUGUI laserCountText;
    [SerializeField] private TextMeshProUGUI hitpointCountText;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private GameObject deathExplosion;
    [SerializeField] private GameObject slowTimeEffect;
    [SerializeField] private Image filledCooldownBox;
    [SerializeField] private TextMeshProUGUI cooldownReadyText;
    [SerializeField] private GameObject slowTimeScreen;
    [SerializeField] private AudioClip bulletFireClip;
    [SerializeField] private AudioClip slowTimeClip;
    private GameManagerScript gameManagerScript;
    private Collider2D laserCollider = null;
    private AudioSource playerBulletAudioSource;
    private AudioSource playerSlowTimeAudioSource;
    private Coroutine slowTimeCoroutine;
    private float moveSpeed = 8f;
    private float xBoundary = 14f;
    private float yBoundary = 13.85f;
    private float bulletTimer = 0f;
    private float bulletCooldown = .1f;
    private float bombTimer = 0f;
    private float bombCooldown = 0.5f;
    private float laserTimer = 0f;
    private float laserCooldown = 7f;
    private float hitpoints;
    private bool hasImmunity = false;
    private int bombCount;
    private int laserCount;
    private bool isLaserOn = false;
    private float laserLifeTime = 2.5f;
    private float laserDamageTimer = 0.25f;
    private float laserDamageCooldown = 0.25f;
    private float damageMultiplier = 1;
    private float timeStopScale = 0.15f;
    private float activeSlowTimer = 0f;
    private float activeSlowDuration = 5f;
    private float slowTimeTimer = 0f;
    private float slowTimeCooldown = 10f;
    private bool slowTimeActive = false;
    private bool makeTrail = false;
    private float makeTrailTimer = 0f;
    private float makeTrailCooldown = 0.05f;

    private void Start()
    {

        Time.timeScale = 1f;

        gameManagerScript = FindAnyObjectByType<GameManagerScript>();

        laserCollider = laser.GetComponent<Collider2D>();
        laser.gameObject.SetActive(false);

        bulletTimer = bulletCooldown;
        bombTimer = bombCooldown;
        laserTimer = laserCooldown;

        slowTimeTimer = 0f;
        cooldownReadyText.enabled = false;

        playerBulletAudioSource = gameObject.AddComponent<AudioSource>();
        playerSlowTimeAudioSource = gameObject.AddComponent<AudioSource>();

        playerBulletAudioSource.clip = bulletFireClip;
        playerBulletAudioSource.volume = 0.05f;
        playerBulletAudioSource.pitch = 1.15f;
        playerBulletAudioSource.playOnAwake = false;

        playerSlowTimeAudioSource.clip = slowTimeClip;
        playerSlowTimeAudioSource.volume = 0.5f;
        playerSlowTimeAudioSource.playOnAwake = false;

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
        transform.position += moveDir * Time.unscaledDeltaTime * moveSpeed;

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

        List<Collider2D> objectsHit = new List<Collider2D>();
        int collision = Physics2D.OverlapCollider(GetComponent<Collider2D>(), new ContactFilter2D().NoFilter(), objectsHit);
        foreach (Collider2D collider in objectsHit)
        {
            HitByObject(collider);
        }

        if (bulletTimer >= bulletCooldown)
        {
            if (Input.GetKey(KeyCode.LeftControl) && !isLaserOn)
            {
                GameObject playerBullet1 = Instantiate(bulletPrefab, playerBulletSpawner.transform.position + new Vector3(-0.15f, 0, 0), transform.rotation);
                GameObject playerBullet2 = Instantiate(bulletPrefab, playerBulletSpawner.transform.position + new Vector3(0.15f, 0, 0), transform.rotation);
                playerBullet1.GetComponent<PlayerBulletScript>().SetDamageMultiplier(damageMultiplier);
                playerBullet2.GetComponent<PlayerBulletScript>().SetDamageMultiplier(damageMultiplier);
                bulletTimer = 0f;
                playerBulletAudioSource.Play();
            } 
        }
        else
        {
            bulletTimer += Time.unscaledDeltaTime;
        }

        if (bombTimer >= bombCooldown)
        {
            if (Input.GetKeyDown(KeyCode.LeftAlt) && !gameManagerScript.IsGameOver())
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
            bombTimer += Time.unscaledDeltaTime;
        }

        if (laserTimer >= laserCooldown)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && !gameManagerScript.IsGameOver())
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
            laserTimer += Time.unscaledDeltaTime;
        }

        if (isLaserOn)
        {
            hasImmunity = true;

            List<Collider2D> results = new List<Collider2D>();
            int numOfCollisions = Physics2D.OverlapCollider(laserCollider, new ContactFilter2D().NoFilter(), results);

            if (laserDamageTimer >= laserDamageCooldown)
            {
                for (int i = 0; i < numOfCollisions; i++)
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
                        results[i].gameObject.GetComponent<EnemyTurretScript>().HitByObject(6);
                    }
                    else if (results[i].gameObject.CompareTag("EnemyDiver"))
                    {
                        results[i].gameObject.GetComponent<EnemyDiverPlaneScript>().HitByObject(6);
                    }
                    else if (results[i].gameObject.CompareTag("EnemySine"))
                    {
                        results[i].gameObject.GetComponent<EnemySinePlaneScript>().HitByObject(4);
                    }
                    else if (results[i].gameObject.CompareTag("EnemyDelayed"))
                    {
                        results[i].gameObject.GetComponent<EnemyDelayedScript>().HitByObject(5);
                    }
                }

                laserDamageTimer = 0f;
            }
            else
            {
                laserDamageTimer += Time.unscaledDeltaTime;
            }

            for (int i = 0; i < numOfCollisions; i++)
            {
                if (results[i].gameObject.CompareTag("EnemyBullet"))
                {
                    Destroy(results[i].gameObject);
                }
            }
        }

        if (!gameManagerScript.IsGameOver())
        {
            if (!slowTimeActive)
            {
                if (slowTimeTimer > slowTimeCooldown)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        activeSlowTimer = activeSlowDuration;
                        slowTimeCoroutine = StartCoroutine(SlowTimeEffect());
                        cooldownReadyText.enabled = false;
                        slowTimeTimer = 0f;
                    }
                    else
                    {
                        cooldownReadyText.enabled = true;
                    }
                }
                else
                {
                    slowTimeTimer += Time.deltaTime;
                    filledCooldownBox.fillAmount = slowTimeTimer / slowTimeCooldown;
                }
            }
            else
            {
                activeSlowTimer -= Time.unscaledDeltaTime;
                filledCooldownBox.fillAmount = activeSlowTimer / activeSlowDuration;
            }
        }
        else
        {
            if (slowTimeActive)
            {
                StopCoroutine(slowTimeCoroutine);
                slowTimeActive = false;
                Time.timeScale = 1f;
                moveSpeed /= 0.75f;
                makeTrail = false;
            }
        }
        
        if (makeTrail)
        {
            if (makeTrailTimer > makeTrailCooldown)
            {
                GameObject currentFrame = Instantiate(slowTimeEffect, transform.position, Quaternion.Euler(0, 0, -90));
                Sprite currentSprite = transform.Find("PlayerVisual").GetComponent<SpriteRenderer>().sprite;
                currentFrame.GetComponent<SpriteRenderer>().sprite = currentSprite;
                
                Destroy(currentFrame, 1f);
                makeTrailTimer = 0f;
            }
            else
            {
                makeTrailTimer += Time.unscaledDeltaTime;
            }
        }
    }

    private IEnumerator SlowTimeEffect()
    {
        slowTimeActive = true;
        Time.timeScale = timeStopScale;
        moveSpeed *= 0.75f;
        playerSlowTimeAudioSource.Play();
        makeTrail = true;
        GameObject screenLayover = Instantiate(slowTimeScreen, Vector3.zero, transform.rotation);

        yield return new WaitForSecondsRealtime(activeSlowDuration);

        slowTimeActive = false;
        Time.timeScale = 1f;
        moveSpeed /= 0.75f;
        makeTrail = false;
        screenLayover.GetComponent<Animator>().SetBool("SlowTimeIsOver", true);
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
                ScreenShakeScript.Instance.Shake(1.25f, 0.75f);
                GameObject deathAnim = Instantiate(deathExplosion, transform.position, transform.rotation);
                deathAnim.GetComponent<AudioSource>().volume = 0.4f;
                gameManagerScript.GameOver();
                Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
            }

            StartCoroutine(HitImmunity(2f));
        }
    }

    public IEnumerator HitImmunity(float immuneTime)
    {
        hasImmunity = true;
        playerAnimator.SetBool("HasImmunity", true);
        yield return new WaitForSeconds(immuneTime);
        playerAnimator.SetBool("HasImmunity", false);
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
            ScoreManagerScript.Instance.ScorePowerup();
        }

        UpdateStats();
    }

    private IEnumerator Laser()
    {
        isLaserOn = true;
        laser.gameObject.SetActive(true);
        laserCollider.enabled = true;
        playerAnimator.SetBool("LaserIsOn", true);
        yield return new WaitForSecondsRealtime(laserLifeTime);
        hasImmunity = false;
        isLaserOn = false;
        laser.gameObject.SetActive(false);
        laserCollider.enabled = false;
        playerAnimator.SetBool("LaserIsOn", false);
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