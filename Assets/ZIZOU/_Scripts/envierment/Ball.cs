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

    [SerializeField] private BallType type;
    [SerializeField] private float fallSpeed = 3f;
    [SerializeField] private float bottomY = -6f;

    private bool consumed;

    public BallType Type => type;

    private void OnEnable() { Active.Add(this); }
    private void OnDisable() { Active.Remove(this); }

    private void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        if (transform.position.y <= bottomY)
        {
            consumed = true;
            ReachedBottom?.Invoke(this);
            Destroy(gameObject);
        }
    }

    public void Kick()
    {
        if (type == BallType.Bomb)
            Explode();
        else
            Collect();
    }

    public void Explode()
    {
        if (consumed) return;
        consumed = true;

        Exploded?.Invoke(this);
        Destroy(gameObject);
    }

    private void Collect()
    {
        if (consumed) return;
        consumed = true;

        Collected?.Invoke(this);
        Destroy(gameObject);
    }
}