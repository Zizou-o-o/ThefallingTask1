using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static event Action<int> ScoreChanged;

    [SerializeField] private int normalPoints = 10;
    [SerializeField] private int slowPoints = 25;

    private int score;

    private void Awake()
    {
        score = 0;
    }

    private void Start()
    {
        ScoreChanged?.Invoke(score);
    }

    private void OnEnable() { Ball.Collected += OnBallCollected; }
    private void OnDisable() { Ball.Collected -= OnBallCollected; }

    private void OnBallCollected(Ball ball)
    {
        score += PointsFor(ball.Type);
        ScoreChanged?.Invoke(score);
    }

    private int PointsFor(BallType type)
    {
        return type == BallType.Slow ? slowPoints : normalPoints;
    }
}