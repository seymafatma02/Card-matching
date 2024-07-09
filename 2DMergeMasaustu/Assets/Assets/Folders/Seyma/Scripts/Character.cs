using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 5f;
    private Rigidbody2D rigidbody2D;
    private Animator animator;

    private bool grounded;
    private bool started;
    private bool jumping;
    private bool facingRight = true;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        grounded = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            if (grounded)
            {
                if (started)
                {
                    Jump();
                }
                else
                {
                    animator.SetBool("GameStarted", true);
                    started = true;
                }
            }
        }

        if (started)
        {
            Move();
        }

        animator.SetBool("Grounded", grounded);
    }

    private void FixedUpdate()
    {
        if (jumping)
        {
            rigidbody2D.AddForce(new Vector2(0f, jumpForce));
            jumping = false;
        }
    }

    private void Move()
    {
        float move = Input.GetAxis("Horizontal");

        animator.SetFloat("Speed", Mathf.Abs(move));

        if (move != 0)
        {
            if (move > 0 && !facingRight)
            {
                Flip();
            }
            else if (move < 0 && facingRight)
            {
                Flip();
            }
        }

        rigidbody2D.velocity = new Vector2(move * speed, rigidbody2D.velocity.y);

        // Update the running state based on movement
        animator.SetBool("Running", move != 0);
    }

    private void Jump()
    {
        rigidbody2D.AddForce(new Vector2(0, jumpForce));
        animator.SetTrigger("Jump");
        grounded = false;
        jumping = true;
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }
    }
}
