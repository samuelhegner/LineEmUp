using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        scoreText.text = RoomManager.Instance.GetScore();
    }

    public void LeaveMatch() 
    {
        Destroy(RoomManager.Instance.gameObject);
        LeaveRoom();
        SceneManager.LoadScene("Menu Scene");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

}
