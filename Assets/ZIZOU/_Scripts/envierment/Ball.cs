using System;
using UnityEngine;

public enum BallType { Slow, Normal, Bomb }

public class Ball : MonoBehaviour
{
    public static event Action<Ball> Collected;
    public static event Action<Ball> ReachedBottom;

    [SerializeField] private BallType type;
    [SerializeField] private float fallSpeed = 3f;
    [SerializeField] private float bottomY = -6f;

    public BallType Type => type;

    private void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        if (transform.position.y <= bottomY)
        {
            ReachedBottom?.Invoke(this);
            Destroy(gameObject);
        }
    }

    public void Collect()
    {
        Collected?.Invoke(this);
        Destroy(gameObject);
    }
}
