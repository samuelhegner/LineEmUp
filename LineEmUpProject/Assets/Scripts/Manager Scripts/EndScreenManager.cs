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



    private void Start()
    {
        scoreText.text = RoomManager.Instance.GetScore(); // Set the highscore to the previous games highscore
        playAgainButton.SetActive(PhotonNetwork.IsMasterClient); //allow host to have controls over restarting game
        backToMenu.SetActive(PhotonNetwork.IsMasterClient); //allow host to have controls over quiting the lobby game
        
        waitingForHostText.SetActive(!PhotonNetwork.IsMasterClient); //inform non host players that they are waiting for the host
        
        if(PhotonNetwork.IsMasterClient)
            PhotonNetwork.DestroyAll(); //Clears the room of all networked objects, needed to restart the game

        photonView.RPC("RPC_DestroyRoomManager", RpcTarget.All); // now that the highscore got set, room manager is no longer needed
    }

    /// <summary>
    /// Leave the current room and return to menu
    /// </summary>
    public void LeaveMatch() 
    {
        photonView.RPC("RPC_DispandMatch", RpcTarget.All); //Make all players leave the lobby
    }

    /// <summary>
    /// Play again with the current players
    /// </summary>
    public void RestartGame() 
    {
        PhotonNetwork.LoadLevel("Game Scene"); // Restart the game for all players
    }

    /// <summary>
    /// Leave the room the local player is in
    /// </summary>
    private void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    
    /// <summary>
    /// Callback for when host is migrating
    /// </summary>
    /// <param name="newMasterClient"></param>
    public override void OnMasterClientSwitched(Player newMasterClient) //allow  new host to have controls over the end game screen
    {
        playAgainButton.SetActive(PhotonNetwork.IsMasterClient);
        backToMenu.SetActive(PhotonNetwork.IsMasterClient);
        waitingForHostText.SetActive(!PhotonNetwork.IsMasterClient);
    }

    /// <summary>
    /// Callback for when the room is left
    /// </summary>
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Menu Scene"); // load the menu, once the room has been left
    }

    [PunRPC]
    void RPC_DispandMatch()
    {
        if (!photonView.IsMine)
            return;
        LeaveRoom();
    }

    [PunRPC]
    void RPC_DestroyRoomManager() 
    {
        if (!photonView.IsMine)
            return;
        PhotonNetwork.Destroy(RoomManager.Instance.gameObject);
    }
}
