using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float secondsToSpawn = 2;


    [SerializeField] private Transform[] spawnLocations;

    int spawnLocationIndex = 0;

    Coroutine spawner;

    void Start()
    {
        spawner = StartCoroutine(SpawnEnemy());
    }

    void StopSpawning() 
    {
        if (spawner != null)
            StopCoroutine(spawner);
    }


    private IEnumerator SpawnEnemy() 
    {
        while (true) 
        {
            Instantiate(enemyPrefab, spawnLocations[spawnLocationIndex].position, Quaternion.identity, transform);
            spawnLocationIndex = (spawnLocationIndex + 1) % spawnLocations.Length;
            yield return new WaitForSeconds(secondsToSpawn);
        }
    }
}
