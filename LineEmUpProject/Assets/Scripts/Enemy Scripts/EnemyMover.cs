using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private Rigidbody enemyRigidbody;
    [SerializeField] private float enemyMoveSpeed;


    private GameObject[] players;
    private Transform targetPlayer;

    void Awake()
    {
        players = GameObject.FindGameObjectsWithTag("Player"); //TODO: Tags are slow, be sure to find players in a better manner
    }

    void FixedUpdate()
    {
        setPlayerTargetToClosestPlayer();
        moveToTargetedPlayer();
    }

    
    /// <summary>
    /// Sets the targeted player to the closest player
    /// </summary>
    private void setPlayerTargetToClosestPlayer()
    {
        targetPlayer = findClosestPlayer();        
    }

    /// <summary>
    /// Calculate which player is the closest to this enemy
    /// </summary>
    /// <returns></returns>
    private Transform findClosestPlayer()
    {
        float closestDistance = float.MaxValue;

        Transform closestPlayer = players[0].transform;

        for (int i = 0; i < players.Length; i++)
        {
            float distanceToCurrentPlayer = Vector3.Distance(players[i].transform.position, transform.position);

            if (playerIsCurrentClosest(distanceToCurrentPlayer, closestDistance)) //Check if the checked player is the closer than any before
            {
                closestDistance = distanceToCurrentPlayer; //update current closest distance
                closestPlayer = players[i].transform; //change the target to the closest player
            }
        }

        return closestPlayer;
    }

    /// <summary>
    /// Return whether a distance value is smaller that the current smallest
    /// </summary>
    /// <param name="newDistanceToPlayer"></param>
    /// <param name="currentClosestDistance"></param>
    /// <returns></returns>
    private bool playerIsCurrentClosest(float newDistanceToPlayer, float currentClosestDistance)
    {
        return newDistanceToPlayer < currentClosestDistance;
    }

    /// <summary>
    /// Move the enemy towards the targetPlayer
    /// </summary>
    private void moveToTargetedPlayer()
    {
        Vector3 directionToTarget = calculateMovementDirection();
        enemyRigidbody.MovePosition(transform.position + (directionToTarget * enemyMoveSpeed * Time.deltaTime));
    }

    /// <summary>
    /// calculate the flat, normalized vector to the target
    /// </summary>
    /// <returns></returns>
    private Vector3 calculateMovementDirection()
    {
        Vector3 toPlayer = targetPlayer.position - transform.position;
        toPlayer.y = 0; //Make sure the direction is flat to the player
        return Vector3.Normalize(toPlayer);
    }
}
