using System;
using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static event Action<int> WaveStarted;

    [SerializeField] private BallSpawner spawner;
    [SerializeField] private int firstWaveTarget = 5;
    [SerializeField] private int targetIncreasePerWave = 3;
    [SerializeField] private float startSpawnInterval = 1.5f;
    [SerializeField] private float intervalDecreasePerWave = 0.1f;
    [SerializeField] private float minSpawnInterval = 0.5f;
    [SerializeField] private float timeBetweenWaves = 3f;

    private int currentWave;
    private int collected;
    private int target;
    private bool waveActive;
    private Coroutine spawnRoutine;

    private void OnEnable() { Ball.Collected += OnBallCollected; }
    private void OnDisable() { Ball.Collected -= OnBallCollected; }

    private void Start()
    {
        BeginWave();
    }

    private void BeginWave()
    {
        currentWave++;
        collected = 0;
        target = firstWaveTarget + targetIncreasePerWave * (currentWave - 1);
        waveActive = true;

        float interval = Mathf.Max(minSpawnInterval,
            startSpawnInterval - intervalDecreasePerWave * (currentWave - 1));

        WaveStarted?.Invoke(currentWave);
        spawnRoutine = StartCoroutine(SpawnLoop(interval));
    }

    private IEnumerator SpawnLoop(float interval)
    {
        while (true)
        {
            spawner.Spawn();
            yield return new WaitForSeconds(interval);
        }
    }

    private void OnBallCollected(Ball ball)
    {
        if (!waveActive || ball.Type == BallType.Bomb) return;

        collected++;

        if (collected >= target)
        {
            waveActive = false;
            StartCoroutine(EndWave());
        }
    }

    private IEnumerator EndWave()
    {
        StopCoroutine(spawnRoutine);
        ClearBalls();
        yield return new WaitForSeconds(timeBetweenWaves);
        BeginWave();
    }

    private void ClearBalls()
    {
        foreach (Ball ball in FindObjectsByType<Ball>(FindObjectsSortMode.None))
            Destroy(ball.gameObject);
    }
}