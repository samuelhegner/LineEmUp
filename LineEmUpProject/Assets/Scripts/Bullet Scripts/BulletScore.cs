using Photon.Pun;
using System.Collections;
using UnityEngine;

public class BulletScore : MonoBehaviour, IPunObservable
{
    private int score = 0;

    private void OnEnable()
    {
        GetComponent<BulletCollision>().enemyHit += addToScore;
        GetComponent<BulletOffScreenChecker>().bulletOutOfBounds += AddScore;
    }

    private void OnDisable()
    {
        GetComponent<BulletCollision>().enemyHit -= addToScore;
        GetComponent<BulletOffScreenChecker>().bulletOutOfBounds -= AddScore;
    }

    private void addToScore() 
    {
        score++;
    }

    private void AddScore() 
    {
        RoomManager.Instance.AddNewScore(score);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //We own this player: send the others our data
            stream.SendNext(score);
        }
        else
        {
            //Network player, receive data
            score = (int)stream.ReceiveNext();
        }
    }
}
