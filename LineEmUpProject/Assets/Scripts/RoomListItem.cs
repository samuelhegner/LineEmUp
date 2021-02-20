using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    RoomInfo roomInfo;

    public void SetUp(RoomInfo info) 
    {
        roomInfo = info;
        text.text = info.Name;
    }

    public void OnClicked() 
    {
        Launcher.Instance.JoinRoom(roomInfo);
    }
}
