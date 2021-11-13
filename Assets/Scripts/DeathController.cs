using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathController : MonoBehaviour
{
    public Transform PlayerSpawnPoint;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.position = PlayerSpawnPoint.position;
        }
        else
        {
            collision.gameObject.SetActive(false);
        }
    }
}
