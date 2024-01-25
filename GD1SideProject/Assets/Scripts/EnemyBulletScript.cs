using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{

    [SerializeField] private bool autoMove = true;
    private float bulletSpeed;
    private float yBulletBoundary = 10f;

    private void Update()
    {
        if (autoMove)
        {
            transform.position += transform.up * bulletSpeed * Time.deltaTime;
        }

        if (transform.position.y < -yBulletBoundary)
        {
            Destroy(gameObject);
        }
    }

    public void SetSpeed(float speed)
    {
        bulletSpeed = speed;
    }

}
