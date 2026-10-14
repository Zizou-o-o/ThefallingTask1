using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{
    public static event Action<int> WaveStarted;

    [SerializeField] private BallSpawner spawner;

    [Header("Counts")]
    [SerializeField] private int firstRoundNormals = 5;
    [SerializeField] private int normalsAddedPerRound = 2;
    [SerializeField] private int bombEveryRounds = 2;
    [SerializeField] private int slowEveryRounds = 3;

    [Header("Speeds")]
    [SerializeField] private float normalBaseSpeed = 3f;
    [SerializeField] private float normalSpeedIncrease = 0.1f;
    [SerializeField] private float slowSpeed = 1.5f;
    [SerializeField] private float bombMinSpeed = 2f;
    [SerializeField] private float bombMaxSpeed = 6f;

    [Header("Timing")]
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private float timeBetweenRounds = 3f;

    [Header("Target")]
    [SerializeField] private Target targetPrefab;
    [SerializeField] private int targetEveryRounds = 5;
    [SerializeField] private float targetLeftX = -8f;
    [SerializeField] private float targetRightX = 8f;

    private int currentRound;

    private void Start()
    {
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        while (true)
        {
            currentRound++;
            WaveStarted?.Invoke(currentRound);

            Target target = TrySpawnTarget(currentRound);

            yield return SpawnRound(currentRound);
            yield return new WaitUntil(RoundIsOver);

            if (target != null)
                Destroy(target.gameObject);

            yield return new WaitForSeconds(timeBetweenRounds);
        }
    }

    private bool RoundIsOver()
    {
        return Ball.Active.Count == 0 && Ball.Kicked.Count == 0;
    }

    private Target TrySpawnTarget(int round)
    {
        if (targetPrefab == null || round % targetEveryRounds != 0)
            return null;

        float x = Random.value < 0.5f ? targetLeftX : targetRightX;
        return Instantiate(targetPrefab, new Vector3(x, 0f, 0f), Quaternion.identity);
    }

    private IEnumerator SpawnRound(int round)
    {
        List<BallType> queue = BuildQueue(round);

        foreach (BallType type in queue)
        {
            spawner.Spawn(type, SpeedFor(type, round));
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private List<BallType> BuildQueue(int round)
    {
        List<BallType> queue = new List<BallType>();

        AddBalls(queue, BallType.Normal, NormalCount(round));
        AddBalls(queue, BallType.Bomb, BombCount(round));
        AddBalls(queue, BallType.Slow, SlowCount(round));

        Shuffle(queue);
        return queue;
    }

    private int NormalCount(int round) => firstRoundNormals + normalsAddedPerRound * (round - 1);
    private int BombCount(int round) => round / bombEveryRounds;
    private int SlowCount(int round) => round / slowEveryRounds;

    private float SpeedFor(BallType type, int round)
    {
        switch (type)
        {
            case BallType.Normal: return normalBaseSpeed + normalSpeedIncrease * (round - 1);
            case BallType.Slow: return slowSpeed;
            default: return Random.Range(bombMinSpeed, bombMaxSpeed);
        }
    }

    private void AddBalls(List<BallType> queue, BallType type, int count)
    {
        for (int i = 0; i < count; i++)
            queue.Add(type);
    }

    private void Shuffle(List<BallType> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}