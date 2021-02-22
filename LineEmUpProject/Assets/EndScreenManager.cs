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
        
        if(PhotonNetwork.IsMasterClient)
            PhotonNetwork.DestroyAll();
        PV.RPC("RPC_DestroyRoomManager", RpcTarget.All);
    }

    public void LeaveMatch() 
    {
        PV.RPC("RPC_DispandMatch", RpcTarget.All);
    }

    public void RestartGame() 
    {
        PhotonNetwork.LoadLevel("Game Scene");
    }

    private void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    [PunRPC]
    void RPC_DispandMatch() 
    {
        if (!photonView.IsMine)
            return;
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

    [PunRPC]
    void RPC_DestroyRoomManager() 
    {
        if (!photonView.IsMine)
            return;
        PhotonNetwork.Destroy(RoomManager.Instance.gameObject);
    }
}
