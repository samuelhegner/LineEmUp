using System.Collections;
using UnityEngine;

public class BulletScore : MonoBehaviour
{
    private int score = 0;

    private void OnEnable()
    {
        GetComponent<BulletCollision>().enemyHit += addToScore;
    }

    private void OnDisable()
    {
        GetComponent<BulletCollision>().enemyHit -= addToScore;
    }

    private void addToScore() 
    {
        score++;
    }
}
