using UnityEngine;
using System.Collections;

public class BombSpawner : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject bombPrefab;

    [Header("Spawn Timing (seconds)")]
    public float minSpawnInterval = 5f;
    public float maxSpawnInterval = 45f;

    [Header("Spawn Area")]
    public float minX = -8f;
    public float maxX = 8f;
    public float spawnY = 6f;

    [Header("Limits")]
    public bool onlyOneAtATime = false;
    public string bombTag = "Bomb";   // set this on the prefab if using onlyOneAtATime

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            float wait = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(wait);

            if (bombPrefab == null) continue;

            if (onlyOneAtATime && GameObject.FindWithTag(bombTag) != null)
                continue;

            float x = Random.Range(minX, maxX);
            Vector3 spawnPos = new Vector3(x, spawnY, 0f);

            Instantiate(bombPrefab, spawnPos, Quaternion.identity);
        }
    }
}

