using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Movement")]
    public float moveForce;
    public Transform lookAtPoint;
    public Transform pointInFront;
    public LayerMask groundLayerMask;
    public LayerMask wallLayerMask;
    public bool isGroundAhead;

    private Rigidbody2D enemyRB;

    // Start is called before the first frame update
    void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        LookAhead();
        LookFront();
        MoveEnemy();
    }

    private void LookAhead()
    {
        var hit = Physics2D.Linecast(transform.position, lookAtPoint.position, groundLayerMask);

        isGroundAhead = (hit) ? true : false;
    }

    private void LookFront()
    {
        var hit = Physics2D.Linecast(transform.position, pointInFront.position, wallLayerMask);

        if (hit)
        {
            FlipEnemy();
        }
    }

    private void MoveEnemy()
    {
        if (isGroundAhead)
        {
            enemyRB.AddForce(Vector2.left * moveForce * transform.localScale.x);
            enemyRB.velocity *= 0.9f;
        }
        else
        {
            FlipEnemy();
        }
    }

    private void FlipEnemy()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1.0f, transform.localScale.y, transform.localScale.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            transform.SetParent(collision.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            transform.SetParent(null);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, lookAtPoint.position);
        Gizmos.DrawLine(transform.position, lookAtPoint.position);
    }
}
