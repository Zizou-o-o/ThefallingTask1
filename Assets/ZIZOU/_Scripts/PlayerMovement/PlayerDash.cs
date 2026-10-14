using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerJump), typeof(PlayerAnimation))]
public class PlayerDash : MonoBehaviour
{
    [SerializeField] private float dashSpeed = 18f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 0.6f;

    private Rigidbody2D rb;
    private PlayerJump jump;
    private PlayerAnimation playerAnimation;
    private bool dashPressed;
    private float dashDirection;
    private float dashTimeLeft;
    private float cooldownLeft;

    public bool IsDashing { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        jump = GetComponent<PlayerJump>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            dashPressed = true;
    }

    private void FixedUpdate()
    {
        cooldownLeft -= Time.fixedDeltaTime;

        if (dashPressed && !IsDashing && cooldownLeft <= 0f && jump.IsGrounded)
            StartDash();

        dashPressed = false;

        if (IsDashing)
            ContinueDash();
    }

    private void StartDash()
    {
        IsDashing = true;
        dashDirection = playerAnimation.FacingDirection;
        dashTimeLeft = dashDuration;
    }

    private void ContinueDash()
    {
        dashTimeLeft -= Time.fixedDeltaTime;

        if (dashTimeLeft <= 0f || !jump.IsGrounded)
        {
            EndDash();
            return;
        }

        rb.linearVelocity = new Vector2(dashDirection * dashSpeed, rb.linearVelocity.y);
    }

    private void EndDash()
    {
        IsDashing = false;
        cooldownLeft = dashCooldown;
    }
}