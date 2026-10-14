using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerDash))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;

    private Rigidbody2D rb;
    private PlayerDash dash;
    private float moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        dash = GetComponent<PlayerDash>();
    }

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        if (dash.IsDashing) return;

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }
}