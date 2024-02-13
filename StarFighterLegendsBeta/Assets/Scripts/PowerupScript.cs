using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupScript : MonoBehaviour
{

    public enum PowerupType
    {
        bomb,
        laser,
        score
    }

    [SerializeField] PowerupType powerupType = PowerupType.bomb;
    private float fallSpeed = 3f;
    private float yBoundary = -15f;

    private void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        if (transform.position.y < yBoundary)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            switch (powerupType)
            {
                case PowerupType.bomb:
                    collision.gameObject.GetComponent<PlayerScript>().GainedPowerup("Bomb");
                    break;
                case PowerupType.laser:
                    collision.gameObject.GetComponent<PlayerScript>().GainedPowerup("Laser");
                    break;
                case PowerupType.score:
                    collision.gameObject.GetComponent<PlayerScript>().GainedPowerup("Score");
                    break;
            }

            Destroy(gameObject);
        }
    }

}