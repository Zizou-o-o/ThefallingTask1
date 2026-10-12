using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerJump : MonoBehaviour
{
    [Header("Jump")]
    [SerializeField] private float jumpSpeed = 12f;
    [SerializeField] private float minJumpHeight = 1f;
    [SerializeField] private float maxJumpHeight = 3.5f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float checkDistance = 0.05f;
    [SerializeField] private bool isGrounded;

    public bool IsGrounded => isGrounded;
    private Rigidbody2D rb;
    private Collider2D col;
    private bool jumpPressed;
    private bool jumpHeld;
    private bool isJumping;
    private bool canJump = true;
    private bool leftGround;
    private float jumpStartY;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            jumpPressed = true;

        jumpHeld = Input.GetKey(KeyCode.Space);
    }

    private void FixedUpdate()
    {
        isGrounded = CheckGrounded();

        UpdateJumpLock();

        if (jumpPressed && canJump && isGrounded)
            StartJump();

        jumpPressed = false;

        if (isJumping)
            ContinueJump();
    }

    private bool CheckGrounded()
    {
        if (rb.linearVelocity.y > 0.01f)
            return false;

        Bounds b = col.bounds;
        Vector2 origin = new Vector2(b.center.x, b.min.y + 0.02f);
        Vector2 size = new Vector2(b.size.x * 0.9f, 0.04f);

        RaycastHit2D hit = Physics2D.BoxCast(origin, size, 0f, Vector2.down, checkDistance, groundLayer);
        return hit.collider != null && !hit.collider.isTrigger;
    }

    private void UpdateJumpLock()
    {
        if (canJump) return;

        if (!isGrounded)
            leftGround = true;
        else if (leftGround && !isJumping)
            canJump = true;
    }

    private void StartJump()
    {
        isJumping = true;
        canJump = false;
        leftGround = false;
        jumpStartY = rb.position.y;
    }

    private void ContinueJump()
    {
        float height = rb.position.y - jumpStartY;

        bool reachedMax = height >= maxJumpHeight;
        bool released = !jumpHeld && height >= minJumpHeight;

        if (reachedMax || released)
            EndJump();
        else
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpSpeed);
    }

    private void EndJump()
    {
        isJumping = false;
        float y = Mathf.Min(rb.linearVelocity.y, 0f);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, y);
    }
}