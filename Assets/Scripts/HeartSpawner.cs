using UnityEngine;
using System.Collections;

public class HeartSpawner : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject heartPrefab;

    [Header("Spawn Timing (seconds)")]
    public float minSpawnInterval = 10f;
    public float maxSpawnInterval = 50f;

    [Header("Spawn Area")]
    public float minX = -8f;
    public float maxX = 8f;
    public float spawnY = 6f;

    [Header("Limits")]
    public bool onlyOneAtATime = true;
    public string heartTag = "Heart";   // Set this on the prefab if using onlyOneAtATime

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

            if (heartPrefab == null) continue;

            if (onlyOneAtATime && GameObject.FindWithTag(heartTag) != null)
                continue;

            float x = Random.Range(minX, maxX);
            Vector3 spawnPos = new Vector3(x, spawnY, 0f);

            Instantiate(heartPrefab, spawnPos, Quaternion.identity);
        }
    }
}
