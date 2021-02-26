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
    [Header("Highscore UI text")]
    [SerializeField] private TMP_Text highScoreText;

    [Header("Players in the room")]
    [SerializeField] private List<Transform> players; //Serialized for debugging purposes

    public static RoomManager Instance; //Singleton for easy practice inplementation


    int highScore; // The highscore of the current game

    public List<Transform> GetPlayers { get => players;}


    private void Awake()
    {
        if (Instance != null) 
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject); // Allows this to carry highscore into endscreen
        Instance = this;
    }

    /// <summary>
    /// Callback when the game scene is loaded
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)//Game Scene
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity); //create the players magager
        }
    }

    /// <summary>
    /// Get the current highscore
    /// </summary>
    /// <returns>The current highscore</returns>
    public string GetScore()
    {
        return highScore.ToString();
    }


 

    /// <summary>
    /// Check if a bullet has hit a new record of enemies
    /// if so, update the highscore
    /// </summary>
    /// <param name="linedUpEnemies"> The ammount of enemies, that a bullet hit</param>
    public void AddNewScore(int linedUpEnemies)
    {
        if (linedUpEnemies > highScore) //new score is a new highscore
        {
            highScore = linedUpEnemies;
            this.photonView.RPC("RPC_UpdateScore", RpcTarget.All, highScore); //update all players highscores
            highScoreText.text = highScore.ToString(); //set the ui text to new score
        }
    }

    [PunRPC] // Updates every players score to the new highscore and displays it
    void RPC_UpdateScore(int score) 
    {
        if (!this.photonView.IsMine)
            return;
        highScore = score;
        highScoreText.text = score.ToString();
    }

    public void AddPlayer(Transform newPlayer)
    {
        players.Add(newPlayer);
    }

    public void RemovePlayer(Transform removedPlayer)
    {
        players.Remove(removedPlayer);
    }


    /// <summary>
    /// Base.OnEnabled is apparently required when inhereting MonoBehaviourPunCallbacks
    /// </summary>
    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    /// <summary>
    /// Base.OnDisabled is apparently required when inhereting MonoBehaviourPunCallbacks
    /// </summary>
    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
