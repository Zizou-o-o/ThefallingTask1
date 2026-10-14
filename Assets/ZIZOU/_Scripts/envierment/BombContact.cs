using UnityEngine;

[RequireComponent(typeof(Ball), typeof(Collider2D))]
public class BombContact : MonoBehaviour
{
    private Ball ball;

    private void Awake()
    {
        ball = GetComponent<Ball>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponentInParent<PlayerMovement>() != null)
            ball.Explode();
    }
}