using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{

    [SerializeField] private bool autoMove = true;
    private float bulletSpeed = 5f;
    private float xBulletBoundary = 15f;
    private float yBulletBoundary = 15f;

    private void Update()
    {
        if (autoMove)
        {
            transform.position += transform.up * bulletSpeed * Time.deltaTime;
        }

        if (transform.position.x < -xBulletBoundary || transform.position.x > xBulletBoundary)
        {
            Destroy(gameObject);
        }

        if (transform.position.y < -yBulletBoundary || transform.position.y > yBulletBoundary)
        {
            Destroy(gameObject);
        }
    }

    public void SetSpeed(float speed)
    {
        bulletSpeed = speed;
    }

    public void SetAutoMove(bool shouldAutoMove)
    {
        autoMove = shouldAutoMove;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

}
