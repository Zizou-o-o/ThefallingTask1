using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] private Ball normalPrefab;
    [SerializeField] private Ball slowPrefab;
    [SerializeField] private Ball bombPrefab;
    [SerializeField] private float minX = -8f;
    [SerializeField] private float maxX = 8f;
    [SerializeField] private float spawnY = 6f;

    public void Spawn(BallType type, float speed)
    {
        Vector3 position = new Vector3(Random.Range(minX, maxX), spawnY, 0f);
        Ball ball = Instantiate(PrefabFor(type), position, Quaternion.identity);
        ball.SetFallSpeed(speed);
    }

    private Ball PrefabFor(BallType type)
    {
        switch (type)
        {
            case BallType.Slow: return slowPrefab;
            case BallType.Bomb: return bombPrefab;
            default: return normalPrefab;
        }
    }
}