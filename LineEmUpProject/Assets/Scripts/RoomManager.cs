using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using TMPro;

public class RoomManager : MonoBehaviourPunCallbacks
{

    [SerializeField] private TMP_Text highScoreText;

    public string GetScore()
    {
        return highScore.ToString();
    }

    [SerializeField] private List<Transform> players;

    public static RoomManager Instance;


    int highScore;

    public List<Transform> GetPlayers { get => players;}

    private void Awake()
    {
        if (Instance) 
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)//Game Scene
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void AddPlayer(Transform newPlayer) 
    {
        players.Add(newPlayer);
    }

    public void RemovePlayer(Transform removedPlayer)
    {
        players.Remove(removedPlayer);
    }

    public void AddNewScore(int linedUpEnemies) 
    {
        if (linedUpEnemies > highScore) 
        {
            highScore = linedUpEnemies;
            highScoreText.text = highScore.ToString();
        }
    }
}
