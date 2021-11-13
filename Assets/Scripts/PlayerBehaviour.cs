using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Movement")]
    public float HorizontalForce;
    public float VerticalForce;
    public bool isGrounded;
    public Transform groundOrigin;
    public float groundOriginRadius;
    public LayerMask groundLayerMask;

    [Header("Animation State")]
    public PlayerAnimationStates state;

    [Range(0.1f, 1.0f)]
    public float airControl;

    private Rigidbody2D playerRB;
    private Animator playerAnimator;



    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        checkIfGrounded();
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");

        if (isGrounded)
        {
            float y = Input.GetAxisRaw("Vertical");
            float jump = Input.GetAxisRaw("Jump");

            if (x != 0)
            {
                FlipPlayer(x);
                playerAnimator.SetInteger("AnimationState", (int) PlayerAnimationStates.RUN);
                state = PlayerAnimationStates.RUN;
            }
            else
            {
                playerAnimator.SetInteger("AnimationState", (int)PlayerAnimationStates.IDLE);
                state = PlayerAnimationStates.IDLE;
            }

            Vector2 worldTouch = new Vector2();

            foreach (var touch in Input.touches)
            {
                worldTouch = Camera.main.ScreenToWorldPoint(touch.position);
            }

            float HorizontalMoveForce = x * HorizontalForce;
            float JumpMoveForce = jump * VerticalForce;

            float PlayerMass = playerRB.mass * playerRB.gravityScale;

            playerRB.AddForce(new Vector2(HorizontalMoveForce, JumpMoveForce) * PlayerMass);
            playerRB.velocity *= 0.98f;
        }
        else
        {
            playerAnimator.SetInteger("AnimationState", (int)PlayerAnimationStates.JUMP);
            state = PlayerAnimationStates.JUMP;

            if (x != 0)
            {
                FlipPlayer(x);

                float HorizontalMoveForce = x * HorizontalForce * airControl;

                float PlayerMass = playerRB.mass * playerRB.gravityScale;

                playerRB.AddForce(new Vector2(HorizontalMoveForce, 0.0f) * PlayerMass);
            }
        }
    }

    private void checkIfGrounded()
    {
        RaycastHit2D hit = Physics2D.CircleCast(groundOrigin.position, groundOriginRadius, Vector2.down, groundOriginRadius, groundLayerMask);

        isGrounded = (hit) ? true : false;
    }

    private float FlipPlayer(float x)
    {
        x = (x > 0) ? 1 : -1;

        transform.localScale = new Vector3(x, 1.0f);
        return x;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundOrigin.position, groundOriginRadius);
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
}
