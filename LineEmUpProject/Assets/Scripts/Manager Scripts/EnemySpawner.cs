using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

/// <summary>
/// Spawn enemies from designated spawn locations
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private Transform[] spawnLocations;
    [SerializeField] private float secondsToSpawn = 2;

    Coroutine spawner;

    int spawnLocationIndex = 0;

    public static EnemySpawner Instance; // Singleton for easy practice implementation


    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    void Start()
    {
        spawner = StartCoroutine(SpawnEnemy()); //Start the spawning process
    }

    /// <summary>
    /// Create an enemy every few seconds at a new spawn location
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnEnemy()
    {
        spawnLocationIndex = Random.Range(0, spawnLocations.Length); //choose a random starting spawn location

        while (true)
        {
            yield return new WaitForSeconds(secondsToSpawn);

            if (RoomManager.Instance.GetPlayers.Count > 0 && PhotonNetwork.IsMasterClient) // if there are players in the room and the local player is the room host
            {
                GameObject newEnemy = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Enemy"), spawnLocations[spawnLocationIndex].position, Quaternion.identity); //create a new enemy
                newEnemy.transform.parent = transform; // parent the new enemy to keep hirearchy clean
                spawnLocationIndex = (spawnLocationIndex + 1) % spawnLocations.Length; // cycle through to the next spawn position
            }
        }
    }
}
