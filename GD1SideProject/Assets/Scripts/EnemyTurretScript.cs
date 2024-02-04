using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurretScript : MonoBehaviour
{

    [SerializeField] private GameObject bullets;
    [SerializeField] private GameObject[] spawners;
    private float bulletSpeed = 5f;
    private float rotateSpeed = 100f;
    private float rotateAmount = 0f;
    private float rotationMultiplier = 20f;
    private float hitpoints = 40f;

    private void Start()
    {
        StartCoroutine(TurretHailFire());
    }

    private void Update()
    {
        rotateAmount += rotationMultiplier * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 10 * rotateAmount), rotateSpeed * Time.deltaTime);
    }

    private IEnumerator TurretHailFire()
    {
        yield return new WaitForSeconds(3f);
        while (true)
        {
            for (int i = 0; i < 6; i++)
            {
                GameObject spawnedBullet = Instantiate(bullets, spawners[i].transform.position, spawners[i].transform.rotation);
                spawnedBullet.GetComponent<EnemyBulletScript>().SetSpeed(bulletSpeed);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    public void HitByObject(string objectName, int damageDone)
    {
        if (objectName == "Bullet")
        {
            hitpoints -= damageDone;
        }
        if (objectName == "Explosion")
        {
            hitpoints -= damageDone;
        }
        if (hitpoints <= 0)
        {
            Destroy(gameObject);
        }
    }

}