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
        Ball.Exploded += OnBallExploded;
    }

    private void OnDisable()
    {
        Ball.ReachedBottom -= OnBallReachedBottom;
        Ball.Exploded -= OnBallExploded;
    }

    private void OnBallReachedBottom(Ball ball)
    {
        if (ball.Type == BallType.Bomb) return;
        LoseLife();
    }

    private void OnBallExploded(Ball ball)
    {
        LoseLife();
    }

    private void LoseLife()
    {
        if (!IsPlaying) return;

        lives--;
        LivesChanged?.Invoke(lives);

        if (lives <= 0)
            EndGame();
    }

    private void EndGame()
    {
        IsPlaying = false;
        Time.timeScale = 0f;
        Debug.Log("Game over: no lives left");
        GameOver?.Invoke();
    }
}