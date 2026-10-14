using UnityEngine;

[RequireComponent(typeof(PlayerJump))]
public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private float shootRange = 3f;

    private PlayerJump jump;

    private void Awake()
    {
        jump = GetComponent<PlayerJump>();
    }

    private void Update()
    {
        if (!GameManager.IsPlaying) return;

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            TryShoot();
    }

    private void TryShoot()
    {
        Ball target = FindNearestBallInRange();

        if (target != null)
            target.Kick();
    }

    private Ball FindNearestBallInRange()
    {
        Ball nearest = null;
        float bestDistance = shootRange;

        foreach (Ball ball in Ball.Active)
        {
            if (ball.Type == BallType.Slow && jump.IsGrounded) continue;

            float distance = Vector2.Distance(transform.position, ball.transform.position);
            if (distance <= bestDistance)
            {
                bestDistance = distance;
                nearest = ball;
            }
        }

        return nearest;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, shootRange);
    }
}