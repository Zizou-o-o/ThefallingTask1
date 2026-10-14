using System;
using System.Collections.Generic;
using UnityEngine;

public enum BallType { Slow, Normal, Bomb }

public class Ball : MonoBehaviour
{
    public static event Action<Ball> Collected;
    public static event Action<Ball> Exploded;
    public static event Action<Ball> ReachedBottom;
    public static readonly List<Ball> Active = new List<Ball>();
    public static readonly List<Ball> Kicked = new List<Ball>();

    [SerializeField] private BallType type;
    [SerializeField] private float fallSpeed = 3f;
    [SerializeField] private float bottomY = -6f;

    [Header("Kick")]
    [SerializeField] private float kickSpeed = 15f;
    [SerializeField] private float kickLifetime = 1f;

    private bool consumed;
    private bool isFlying;
    private float flyDirection;

    public BallType Type => type;

    private void OnEnable() { Active.Add(this); }

    private void OnDisable()
    {
        Active.Remove(this);
        Kicked.Remove(this);
    }

    private void Update()
    {
        if (isFlying)
        {
            transform.Translate(Vector3.right * flyDirection * kickSpeed * Time.deltaTime);
            return;
        }

        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        if (transform.position.y <= bottomY)
        {
            consumed = true;
            ReachedBottom?.Invoke(this);
            Destroy(gameObject);
        }
    }

    public void SetFallSpeed(float speed)
    {
        fallSpeed = speed;
    }

    public void Kick(float direction)
    {
        if (type == BallType.Bomb)
            Explode();
        else
            Collect(direction);
    }

    public void Explode()
    {
        if (consumed) return;
        consumed = true;

        Exploded?.Invoke(this);
        Destroy(gameObject);
    }

    private void Collect(float direction)
    {
        if (consumed) return;
        consumed = true;

        Active.Remove(this);
        Kicked.Add(this);
        Collected?.Invoke(this);

        flyDirection = direction;
        isFlying = true;
        Destroy(gameObject, kickLifetime);
    }
}