using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{

    [SerializeField] private bool autoMove = true;
    private float bulletSpeed;
    private float xBulletBoundary = 25f;
    private float yBulletBoundary = 25f;

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

}
