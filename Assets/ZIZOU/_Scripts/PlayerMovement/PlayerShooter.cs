using UnityEngine;

[RequireComponent(typeof(PlayerJump))]
public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private float shootRange = 3f;
    [SerializeField] private bool debugLogs = true;

    private Camera cam;
    private PlayerJump jump;

    private void Awake()
    {
        cam = Camera.main;
        jump = GetComponent<PlayerJump>();
    }

    private void Update()
    {
        if (!GameManager.IsPlaying) return;

        if (Input.GetMouseButtonDown(0))
            TryShoot();
    }

    private void TryShoot()
    {
        if (cam == null)
        {
            Log("No camera found. Set your camera's Tag to MainCamera.");
            return;
        }

        Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        Collider2D[] hits = Physics2D.OverlapPointAll(mouseWorld);

        if (hits.Length == 0)
        {
            Log("No collider under the mouse at " + mouseWorld);
            return;
        }

        foreach (Collider2D hit in hits)
        {
            Ball ball = hit.GetComponentInParent<Ball>();
            if (ball == null) continue;

            float distance = Vector2.Distance(transform.position, ball.transform.position);
            if (distance > shootRange)
            {
                Log("Too far (" + distance.ToString("F1") + " > " + shootRange + ")");
                return;
            }

            if (ball.Type == BallType.Slow && jump.IsGrounded)
            {
                Log("Slow ball can only be hit while the player is in the air");
                return;
            }

            ball.Collect();
            return;
        }

        Log("Mouse is over colliders, but none of them is a Ball");
    }

    private void Log(string message)
    {
        if (debugLogs)
            Debug.Log("Shoot failed: " + message);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, shootRange);
    }
}