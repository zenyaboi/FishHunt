using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private GameObject fishPrefab;
    [SerializeField] private float currentTime;
    [SerializeField] private float spawnTime;
    private float minTime = 1f;
    private float maxTime = 5f;

    void Start()
    {
        SetRandomTime();
        currentTime = minTime;
    }

    void FixedUpdate()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= spawnTime) {
            SpawnObject();
            SetRandomTime();
        }
    }

    void SpawnObject() {
        currentTime = 0;
        Vector2 randomSpawnPos = new Vector2(Random.Range(-37, 38), Random.Range(-5, -14));
        Instantiate(fishPrefab, randomSpawnPos, Quaternion.identity);
    }

    void SetRandomTime() {
        spawnTime = Random.Range(minTime, maxTime);
    }
}
