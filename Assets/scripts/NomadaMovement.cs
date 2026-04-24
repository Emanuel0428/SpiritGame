using UnityEngine;
using UnityEngine.InputSystem;

public class NomadaMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private float horizontal;

    public float speed = 2f;
    public float jumpForce = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        horizontal = 0f;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        {
            horizontal = -1f;
        }

        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            horizontal = 1f;
        }

        if (horizontal < 0.0f ) {
            transform.localScale = new Vector3(-0.05926f, 0.05643f, 0.05f);
        } else if (horizontal > 0.0f) {
            transform.localScale = new Vector3(0.05926f, 0.05643f, 0.05f);
        }

        // Animator DESPUÉS del input
        animator.SetBool("running", horizontal != 0f);

        // salto con W o flecha arriba
        if ((Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
            && Mathf.Abs(rb.linearVelocity.y) < 0.001f)
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }
}