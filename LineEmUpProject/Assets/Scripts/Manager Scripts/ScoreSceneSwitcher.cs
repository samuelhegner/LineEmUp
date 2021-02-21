using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreSceneSwitcher : MonoBehaviour
{
    public static void SwitchToEndScreen() 
    {
        PhotonNetwork.LoadLevel(2);//End Screen
    }
}
