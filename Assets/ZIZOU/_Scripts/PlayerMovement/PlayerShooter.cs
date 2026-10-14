using UnityEngine;

[RequireComponent(typeof(PlayerJump), typeof(PlayerAnimation))]
public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private float shootRange = 3f;

    private PlayerJump jump;
    private PlayerAnimation playerAnimation;

    private void Awake()
    {
        jump = GetComponent<PlayerJump>();
        playerAnimation = GetComponent<PlayerAnimation>();
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
            target.Kick(playerAnimation.FacingDirection);
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