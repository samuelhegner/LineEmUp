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

    

    [SerializeField] private List<Transform> players;

    public static RoomManager Instance;


    int highScore;

    public List<Transform> GetPlayers { get => players;}

    PhotonView PV;

    private void Awake()
    {
        if (Instance != null) 
        {
            print("Duplicate Room Manager Found!");
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;

        PV = GetComponent<PhotonView>();
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

    public string GetScore()
    {
        return highScore.ToString();
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
            PV.RPC("RPC_UpdateScore", RpcTarget.All, highScore);
            highScoreText.text = highScore.ToString();
        }
    }

    [PunRPC]
    void RPC_UpdateScore(int score) 
    {
        if (!photonView.IsMine)
            return;
        highScore = score;
        highScoreText.text = score.ToString();
    }
}
