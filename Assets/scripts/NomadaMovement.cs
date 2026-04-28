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
    public float groundCheckDistance = 0.3f;
    public float groundOffsetY = -0.11f;
    public LayerMask groundLayer;

    [Header("Dash")]
    public float dashForce = 4f;
    public float dashCooldown = 2f;
    public float dashDuration = 0.15f;
    public float jumpAfterDashDelay = 0.1f;
    private float lastDashTime = -999f;
    private float lastDashEndTime = -999f;
    private bool isDashing = false;
    private float dashTimer = 0f;
    private float dashDirection = 1f;
    private Vector3 originalScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        GroundCheck();

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f)
            {
                isDashing = false;
                lastDashEndTime = Time.time;
            }
            return;
        }

        HandleMovement();
        HandleJump();
        HandleDash();
    }

    void FixedUpdate()
    {
        if (isDashing) return;
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }

    private void GroundCheck()
    {
        Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y + groundOffsetY);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, groundCheckDistance, groundLayer);
        isGrounded = hit.collider != null;
        animator.SetBool("isGrounded", isGrounded);
        Debug.DrawRay(rayOrigin, Vector2.down * groundCheckDistance, Color.red);
    }

    private void HandleMovement()
    {
        horizontal = 0f;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        {
            horizontal = -1f;
            dashDirection = -1f;
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            horizontal = 1f;
            dashDirection = 1f;
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }

        animator.SetBool("running", horizontal != 0f);
    }

    private void HandleJump()
    {
        bool canJump = isGrounded && (Time.time - lastDashEndTime >= jumpAfterDashDelay);

        if ((Keyboard.current.wKey.wasPressedThisFrame ||
             Keyboard.current.upArrowKey.wasPressedThisFrame) && canJump)
        {
            animator.SetTrigger("Jump");
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }

    private void HandleDash()
    {
        if (!Keyboard.current.leftShiftKey.wasPressedThisFrame) return;

        if (Time.time - lastDashTime >= dashCooldown)
        {
            lastDashTime = Time.time;
            isDashing = true;
            dashTimer = dashDuration;
            rb.linearVelocity = new Vector2(dashDirection * dashForce, rb.linearVelocity.y);
            animator.SetTrigger("Dash");
        }
    }

    void OnDrawGizmos()
    {
        Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y + groundOffsetY);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(rayOrigin, rayOrigin + Vector2.down * groundCheckDistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(rayOrigin, 0.005f);
    }
}