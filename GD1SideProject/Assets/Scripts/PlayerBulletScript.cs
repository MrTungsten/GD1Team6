using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletScript : MonoBehaviour
{

    private float bulletSpeed = 15f;
    private float yBulletBoundary = 10f;

    private void Update()
    {
        transform.position += transform.up * Time.deltaTime * bulletSpeed;

        if (transform.position.y > yBulletBoundary)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyPlane"))
        {
            collision.gameObject.GetComponent<EnemyPlaneScript>().HitByObject("Bullet");
        }
        else if (collision.gameObject.CompareTag("EnemyTank"))
        {
            collision.gameObject.GetComponent<EnemyTankScript>().HitByObject("Bullet");
        }

        Destroy(gameObject);
    }

}
