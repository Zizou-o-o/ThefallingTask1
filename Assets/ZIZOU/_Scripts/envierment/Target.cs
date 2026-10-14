using System;
using UnityEngine;

public class Target : MonoBehaviour
{
    public static event Action<Target> Hit;

    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float minY = -3f;
    [SerializeField] private float maxY = 3f;
    [SerializeField] private float hitRadius = 0.7f;

    private float direction = 1f;

    private void Update()
    {
        Move();
        CheckHit();
    }

    private void Move()
    {
        transform.Translate(Vector3.up * direction * moveSpeed * Time.deltaTime);

        float y = transform.position.y;

        if (y >= maxY)
            direction = -1f;
        else if (y <= minY)
            direction = 1f;
    }

    private void CheckHit()
    {
        foreach (Ball ball in Ball.Kicked)
        {
            float distance = Vector2.Distance(transform.position, ball.transform.position);
            if (distance > hitRadius) continue;

            Hit?.Invoke(this);
            Destroy(ball.gameObject);
            return;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hitRadius);
    }
}