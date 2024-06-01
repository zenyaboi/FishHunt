using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private GameObject fishPrefab;
        
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Vector2 randomSpawnPos = new Vector2(Random.Range(-37, 38), Random.Range(-3, -14));
            Instantiate(fishPrefab, randomSpawnPos, Quaternion.identity);
        }
    }
}
