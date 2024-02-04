using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameManagerScript gameManagerScript;
    [SerializeField] private GameObject playerBulletSpawner;
    [SerializeField] private GameObject bombPrefab;
    private float moveSpeed = 8f;
    private float xBoundary = 6.35f;
    private float yBoundary = 6.7f;
    private float timer = .3f;
    private float bulletCooldown = .3f;
    private float hitpoints = 5f;
    private bool hasImmunity = false;
    private int bombCount = 3;

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

        if (timer >= bulletCooldown)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                Instantiate(bulletPrefab, playerBulletSpawner.transform.position + new Vector3(-0.15f, 0, 0), transform.rotation);
                Instantiate(bulletPrefab, playerBulletSpawner.transform.position + new Vector3(0.15f, 0, 0), transform.rotation);
                timer = 0f;
            } 
        }
        else
        {
            timer += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (bombCount > 0)
            {
                Instantiate(bombPrefab, transform.position, transform.rotation);
                bombCount--;
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasImmunity)
        {
            hitpoints--;
            Debug.Log("Player has been hit! Hitpoints left: " + hitpoints);

            if (hitpoints == 0)
            {
                Debug.Log("Player has lost!");
                gameManagerScript.GameOver();
                Destroy(gameObject);
            }

            StartCoroutine(HitImmunity(0.1f));
        }
    }

    private IEnumerator HitImmunity(float immunityTime)
    {
        hasImmunity = true;
        yield return new WaitForSeconds(immunityTime);
        hasImmunity = false;
    }
}