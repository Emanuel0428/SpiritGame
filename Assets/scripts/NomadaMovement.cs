using UnityEngine;
using UnityEngine.InputSystem;

public class NomadaMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private float horizontal;
    private bool isGrounded;

    [Header("Movement")]
    public float speed = 2f;
    public float jumpForce = 2f;

    [Header("Ground Check")]
    public float groundCheckDistance = 0.04f;
    public float groundOffsetY = -0.11f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // -------------------
        // Movimiento horizontal
        // -------------------
        horizontal = 0f;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            horizontal = -1f;

        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            horizontal = 1f;

        // -------------------
        // Girar personaje
        // -------------------
        if (horizontal < 0f)
            transform.localScale = new Vector3(-0.05926f, 0.05643f, 0.05f);
        else if (horizontal > 0f)
            transform.localScale = new Vector3(0.05926f, 0.05643f, 0.05f);

        // -------------------
        // Ground Check con Raycast
        // -------------------
        Vector2 rayOrigin = new Vector2(
            transform.position.x,
            transform.position.y + groundOffsetY
        );

        Debug.DrawRay(
            rayOrigin,
            Vector2.down * groundCheckDistance,
            Color.red
        );

        RaycastHit2D hit = Physics2D.Raycast(
            rayOrigin,
            Vector2.down,
            groundCheckDistance
        );

        isGrounded = hit.collider != null;

        Debug.Log($"isGrounded: {isGrounded} | Hit: {(hit.collider != null ? hit.collider.name : "null")}");

        // -------------------
        // Animator
        // -------------------
        animator.SetBool("running", horizontal != 0f);
        animator.SetBool("isGrounded", isGrounded);

        // -------------------
        // Salto
        // -------------------
        if ((Keyboard.current.wKey.wasPressedThisFrame ||
             Keyboard.current.upArrowKey.wasPressedThisFrame)
            && isGrounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
        animator.SetTrigger("Jump");
        rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(
            horizontal * speed,
            rb.linearVelocity.y
        );
    }

    void OnDrawGizmos()
    {
        Vector2 rayOrigin = new Vector2(
            transform.position.x,
            transform.position.y + groundOffsetY
        );

        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            rayOrigin,
            rayOrigin + Vector2.down * groundCheckDistance
        );

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(rayOrigin, 0.005f);
    }
}