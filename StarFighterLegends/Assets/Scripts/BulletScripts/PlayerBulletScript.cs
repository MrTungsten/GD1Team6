using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletScript : MonoBehaviour
{

    private float damageMultiplier = 1;
    private float bulletSpeed = 25f;
    private float yBulletBoundary = 10.25f;

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
            collision.gameObject.GetComponent<EnemyPlaneScript>().HitByObject(1 * damageMultiplier);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("EnemyTank"))
        {
            collision.gameObject.GetComponent<EnemyTankScript>().HitByObject(1 * damageMultiplier);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("EnemyTurret"))
        {
            collision.gameObject.GetComponent<EnemyTurretScript>().HitByObject(1.5f * damageMultiplier);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("EnemyDiver"))
        {
            collision.gameObject.GetComponent<EnemyDiverPlaneScript>().HitByObject(1f * damageMultiplier);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("EnemySine"))
        {
            collision.gameObject.GetComponent<EnemySinePlaneScript>().HitByObject(1f * damageMultiplier);
            Destroy(gameObject);
        }
    }

    public void SetDamageMultiplier(float _damageMultiplier)
    {
        damageMultiplier = _damageMultiplier;
    }

}
