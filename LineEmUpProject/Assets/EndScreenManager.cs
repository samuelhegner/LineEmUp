using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndScreenManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;


    private void Start()
    {
        scoreText.text = RoomManager.Instance.GetScore();
    }

    public void RestartGame() 
    {
        Destroy(RoomManager.Instance.gameObject);
        PhotonNetwork.LoadLevel("Game Scene");
    }
    public void LeaveMatch() 
    {
        Destroy(RoomManager.Instance.gameObject);
        LeaveRoom();
        PhotonNetwork.LoadLevel("Menu Scene");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
