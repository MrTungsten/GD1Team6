using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPowerScript : MonoBehaviour
{

    [SerializeField] private GameObject payload;
    [SerializeField] private GameObject explosionBubble;
    [SerializeField] private AudioClip explosionSound;
    private Rigidbody2D payloadRb;
    private CircleCollider2D explosionCircleCollider;
    private float launchForce = 7.5f;
    private float payloadLifetime = 2;
    private float explosionLifetime = 2f;
    private float expansionSize = 10f;
    private float timer = 0f;
    private float explosionCheckTime = 0.5f;

    private void Start()
    {
        payloadRb = GetComponent<Rigidbody2D>();
        payloadRb.AddForce(transform.up * launchForce, ForceMode2D.Impulse);

        explosionCircleCollider = GetComponent<CircleCollider2D>();
        explosionCircleCollider.enabled = false;

        payload.SetActive(true);
        explosionBubble.SetActive(false);

        StartCoroutine(BombMechanism());

    }

    private IEnumerator BombMechanism()
    {
        payload.SetActive(true);
        yield return new WaitForSeconds(payloadLifetime);
        payload.SetActive(false);

        payloadRb.velocity = Vector3.zero;
        explosionBubble.SetActive(true);
        explosionCircleCollider.enabled = true;

        float elapsedTime = 0f;
        float originalRadius = explosionCircleCollider.radius;

        AudioSource.PlayClipAtPoint(explosionSound, transform.position);

        while (elapsedTime < (explosionLifetime / 2))
        {
            float currentRadius = Mathf.Lerp(originalRadius, originalRadius * expansionSize, elapsedTime / (explosionLifetime / 4));
            transform.localScale = new Vector3(1, 1, 0) * currentRadius;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(explosionLifetime);

        Destroy(this.gameObject);
    }

    private void Update()
    {
        if (explosionCircleCollider.isActiveAndEnabled)
        {
            if (timer > explosionCheckTime)
            {
                List<Collider2D> results = new List<Collider2D>();
                int numOfCollisions = Physics2D.OverlapCollider(explosionCircleCollider, new ContactFilter2D().NoFilter(), results);

                for (int i = 0; i < numOfCollisions; i++)
                {
                    if (results[i].gameObject.layer == 7)
                    {
                        if (results[i].gameObject.CompareTag("EnemyPlane"))
                        {
                            results[i].gameObject.GetComponent<EnemyPlaneScript>().HitByObject(2);
                        }
                        else if (results[i].gameObject.CompareTag("EnemyTank"))
                        {
                            results[i].gameObject.GetComponent<EnemyTankScript>().HitByObject(5);
                        }
                        else if (results[i].gameObject.CompareTag("EnemyTurret"))
                        {
                            results[i].gameObject.GetComponent<EnemyTurretScript>().HitByObject(3);
                        }
                        else if (results[i].gameObject.CompareTag("EnemyDiver"))
                        {
                            results[i].gameObject.GetComponent<EnemyDiverPlaneScript>().HitByObject(2);
                        }
                    }
                }

                timer = 0f;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }

}