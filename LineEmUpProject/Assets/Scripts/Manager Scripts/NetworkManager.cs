using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;



/// <summary>
/// This class takes care of all photon network needs in the menus
/// TODO: Split this into smaller classes with more specific functionality rather than this one large script
/// </summary>
public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("Room Creation")]
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] TMP_Text errorText;

    [Header("Room List")]
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;

    [Header("Player List")]
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject playerListItemPrefab;

    [Header("Room Cotrols")]
    [SerializeField] GameObject startGameButton;

    
    public static NetworkManager Instance; //Singleton for quick practice implementation


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings(); // Needed to utilise Photon
    }

    /// <summary>
    /// Create a Room if the player has entered a room name
    /// </summary>
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
            return;

        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.Instance.SwitchMenu("joining room"); // Switch menu to display joining screen
    }

    /// <summary>
    /// Join a given room
    /// </summary>
    /// <param name="info">RoomInfo for the room the client wants to join</param>
    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.SwitchMenu("joining room");// Switch menu to display joining screen
    }

    /// <summary>
    /// Leave the room you are currently in
    /// </summary>
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.SwitchMenu("leaving room");
    }

    /// <summary>
    /// Load the game level for all players in the lobby
    /// </summary>
    public void StartGame()
    {
        PhotonNetwork.LoadLevel("Game Scene");
    }



    /// <summary>
    /// Callback when the client joins the Master network
    /// </summary>
    public override void OnConnectedToMaster()
    {
        Debug.Log("Master Joined");
        PhotonNetwork.JoinLobby(); //Client needs to have joined a lobby in order to join rooms
        PhotonNetwork.AutomaticallySyncScene = true; // Allows host to move all players in Room into game scene
    }

    /// <summary>
    /// Callback once the player has joined the lobby
    /// </summary>
    public override void OnJoinedLobby()
    {
        MenuManager.Instance.SwitchMenu("title");
        PhotonNetwork.NickName = "Player" + Random.Range(0, 100).ToString();
        Debug.Log("Lobby Joined");
    }

    
    /// <summary>
    /// Callback when the player joins a room
    /// </summary>
    public override void OnJoinedRoom()
    {
        MenuManager.Instance.SwitchMenu("room"); //Switch to the room menu
        
        roomNameText.text = PhotonNetwork.CurrentRoom.Name; // Lable the room with the selected room name

        Player[] players = PhotonNetwork.PlayerList; // get a list of all of the players within the room

        for (int i = 0; i < players.Length; i++) // populate the player list with all current room members
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient); //enable the start button for the room host
    }

    /// <summary>
    /// Callback if the client fails to create a room
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message">Error message for the issue that occured</param>
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation Failed: " + message; //Display the error message in the error 
        MenuManager.Instance.SwitchMenu("error"); //Switch to the error screen
    }


    /// <summary>
    /// Callback for when a client leaves a room
    /// </summary>
    public override void OnLeftRoom()
    {
        MenuManager.Instance.SwitchMenu("title"); // Switch to the Title menu

        foreach (Transform trans in playerListContent)
        {
            Destroy(trans.gameObject); // clear the player list once the room is left
        }
    }

    /// <summary>
    /// Callback for when the list of rooms in the lobby changes
    /// </summary>
    /// <param name="roomList"></param>
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in roomListContent) //clear room list
        {
            Destroy(trans.gameObject);
        }
        for (int i = 0; i < roomList.Count; i++) //repopulate the list of rooms in the join room menu
        {
            if (roomList[i].RemovedFromList)
                continue;
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    /// <summary>
    /// Callback when the player joins a room
    /// </summary>
    /// <param name="newPlayer"></param>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer); //
    }

    /// <summary>
    /// Callback when the room host leaves and the host migrates
    /// </summary>
    /// <param name="newMasterClient"> The new room host</param>
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient); // enable the start game button for the Room host
    }
}
