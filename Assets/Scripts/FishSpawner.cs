using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    [SerializeField] private GameObject fishPrefab;
    [SerializeField] private Transform parent;
    public List<GameObject> fishList;
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
        if (!playerController.hasWon) {
            currentTime += Time.deltaTime;

            if (currentTime >= spawnTime) {
                SpawnObject();
                SetRandomTime();
            }
        }

        for (int i = 0; i < fishList.Count; i++) {
            if (fishList[i] == null) {
                fishList.RemoveAt(i);
            }
        }
    }

    void SpawnObject() {
        currentTime = 0;
        Vector2 randomSpawnPos = new Vector2(Random.Range(-37, 38), Random.Range(-5, -14));
        GameObject fishInst = Instantiate(fishPrefab, randomSpawnPos, Quaternion.identity);
        fishInst.transform.SetParent(parent, false);
        fishList.Add(fishInst);
    }

    void SetRandomTime() {
        spawnTime = Random.Range(minTime, maxTime);
    }
}
