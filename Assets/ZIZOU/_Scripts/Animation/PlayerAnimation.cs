using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerJump))]
public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");

    private Rigidbody2D rb;
    private PlayerJump jump;
    private bool facingLeft;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        jump = GetComponent<PlayerJump>();
    }

    private void Update()
    {
        animator.SetFloat(SpeedHash, Mathf.Abs(rb.linearVelocity.x));
        animator.SetBool(IsGroundedHash, jump.IsGrounded);

        UpdateFacing();
    }

    private void LateUpdate()
    {
        spriteRenderer.flipX = facingLeft;
    }

    private void UpdateFacing()
    {
        float input = Input.GetAxisRaw("Horizontal");

        if (input < 0f)
            facingLeft = true;
        else if (input > 0f)
            facingLeft = false;
    }
}