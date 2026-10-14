using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerJump))]
public class PlayerDash : MonoBehaviour
{
    [SerializeField] private float dashSpeed = 18f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 0.6f;

    private Rigidbody2D rb;
    private PlayerJump jump;
    private bool dashPressed;
    private float facing = 1f;
    private float dashDirection;
    private float dashTimeLeft;
    private float cooldownLeft;

    public bool IsDashing { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        jump = GetComponent<PlayerJump>();
    }

    private void Update()
    {
        float input = Input.GetAxisRaw("Horizontal");
        if (input != 0f)
            facing = Mathf.Sign(input);

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
        dashDirection = facing;
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
