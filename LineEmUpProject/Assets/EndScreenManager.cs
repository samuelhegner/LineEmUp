using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text scoreText;

    [SerializeField] private GameObject playAgainButton;
    [SerializeField] private GameObject backToMenu;
    [SerializeField] private GameObject waitingForHostText;



    PhotonView PV;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
        scoreText.text = RoomManager.Instance.GetScore();
        playAgainButton.SetActive(PhotonNetwork.IsMasterClient);
        backToMenu.SetActive(PhotonNetwork.IsMasterClient);
        waitingForHostText.SetActive(!PhotonNetwork.IsMasterClient);
    }

    public void LeaveMatch() 
    {
        PV.RPC("RPC_DispandMatch", RpcTarget.All);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    [PunRPC]
    void RPC_DispandMatch() 
    {
        Destroy(RoomManager.Instance.gameObject);
        LeaveRoom();   
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        playAgainButton.SetActive(PhotonNetwork.IsMasterClient);
        backToMenu.SetActive(PhotonNetwork.IsMasterClient);
        waitingForHostText.SetActive(!PhotonNetwork.IsMasterClient);
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Menu Scene");
    }
}
