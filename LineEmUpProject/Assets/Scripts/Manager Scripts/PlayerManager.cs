using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using System;


/// <summary>
/// Class that instantiates player controllers, will be useful later if players can respawn within the same game
/// </summary>
public class PlayerManager : MonoBehaviour
{
    PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (photonView.IsMine) //only runs if the local player owns this instance
        {
            CreateController();
        }
    }


    void CreateController() 
    {
        Vector3 spawnPoint = new Vector3(UnityEngine.Random.Range(-10, 10), 0, UnityEngine.Random.Range(-10, 10)); //spawns a player at a random location
        GameObject player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnPoint, Quaternion.identity); // Gets the player prefabs and creates it
    }
}
