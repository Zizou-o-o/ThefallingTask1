using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action<int> LivesChanged;
    public static event Action GameOver;
    public static bool IsPlaying { get; private set; } = true;

    [SerializeField] private int startingLives = 3;

    private int lives;

    private void Awake()
    {
        IsPlaying = true;
        Time.timeScale = 1f;
        lives = startingLives;
    }

    private void Start()
    {
        LivesChanged?.Invoke(lives);
    }

    private void OnEnable()
    {
        Ball.ReachedBottom += OnBallReachedBottom;
        Ball.Collected += OnBallCollected;
    }

    private void OnDisable()
    {
        Ball.ReachedBottom -= OnBallReachedBottom;
        Ball.Collected -= OnBallCollected;
    }

    private void OnBallReachedBottom(Ball ball)
    {
        if (!IsPlaying || ball.Type == BallType.Bomb) return;

        lives--;
        LivesChanged?.Invoke(lives);

        if (lives <= 0)
            EndGame("No lives left");
    }

    private void OnBallCollected(Ball ball)
    {
        if (ball.Type == BallType.Bomb)
            EndGame("You got a bomb");
    }

    private void EndGame(string reason)
    {
        if (!IsPlaying) return;

        IsPlaying = false;
        Time.timeScale = 0f;
        Debug.Log("Game over: " + reason);
        GameOver?.Invoke();
    }
}