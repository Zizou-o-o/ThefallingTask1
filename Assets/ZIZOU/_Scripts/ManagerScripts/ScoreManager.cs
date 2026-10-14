using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static event Action<int> ScoreChanged;

    [SerializeField] private int normalPoints = 10;
    [SerializeField] private int slowPoints = 25;
    [SerializeField] private int targetPoints = 10;

    private int score;

    private void Awake()
    {
        score = 0;
    }

    private void Start()
    {
        ScoreChanged?.Invoke(score);
    }

    private void OnEnable()
    {
        Ball.Collected += OnBallCollected;
        Target.Hit += OnTargetHit;
    }

    private void OnDisable()
    {
        Ball.Collected -= OnBallCollected;
        Target.Hit -= OnTargetHit;
    }

    private void OnBallCollected(Ball ball)
    {
        AddPoints(PointsFor(ball.Type));
    }

    private void OnTargetHit(Target target)
    {
        AddPoints(targetPoints);
    }

    private void AddPoints(int points)
    {
        score += points;
        ScoreChanged?.Invoke(score);
    }

    private int PointsFor(BallType type)
    {
        return type == BallType.Slow ? slowPoints : normalPoints;
    }
}