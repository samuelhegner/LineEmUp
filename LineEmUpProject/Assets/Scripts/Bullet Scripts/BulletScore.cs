using System.Collections;
using UnityEngine;

public class BulletScore : MonoBehaviour
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
}
