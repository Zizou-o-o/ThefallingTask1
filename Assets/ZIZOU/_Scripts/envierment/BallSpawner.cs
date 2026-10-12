using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] private Ball[] ballPrefabs;
    [SerializeField] private float minX = -8f;
    [SerializeField] private float maxX = 8f;
    [SerializeField] private float spawnY = 6f;

    public void Spawn()
    {
        Ball prefab = ballPrefabs[Random.Range(0, ballPrefabs.Length)];
        Vector3 pos = new Vector3(Random.Range(minX, maxX), spawnY, 0f);
        Instantiate(prefab, pos, Quaternion.identity);
    }
}