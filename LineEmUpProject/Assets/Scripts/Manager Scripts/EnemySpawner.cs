using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float secondsToSpawn = 2;
    [SerializeField] private Transform[] spawnLocations;

    int spawnLocationIndex = 0;

    public static EnemySpawner Instance;

    Coroutine spawner;

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
            yield return new WaitForSeconds(secondsToSpawn);

            if (RoomManager.Instance.GetPlayers.Count > 0 && PhotonNetwork.IsMasterClient)
            {
                GameObject newEnemy = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Enemy"), spawnLocations[spawnLocationIndex].position, Quaternion.identity);
                newEnemy.transform.parent = transform;
                spawnLocationIndex = (spawnLocationIndex + 1) % spawnLocations.Length;
            }
        }
    }
}
