using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private Rigidbody enemyRigidbody;
    [SerializeField] private float enemyMoveSpeed;


    private Transform targetPlayer;

    void Awake()
    {
    }

    void FixedUpdate()
    {
        if (RoomManager.Instance.GetPlayers.Count == 0) 
        {
            Destroy(gameObject);
        }
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

        Transform closestPlayer = RoomManager.Instance.GetPlayers[0].transform;

        for (int i = 0; i < RoomManager.Instance.GetPlayers.Count; i++)
        {
            float distanceToCurrentPlayer = Vector3.Distance(RoomManager.Instance.GetPlayers[i].transform.position, transform.position);

            if (playerIsCurrentClosest(distanceToCurrentPlayer, closestDistance)) //Check if the checked player is the closer than any before
            {
                closestDistance = distanceToCurrentPlayer; //update current closest distance
                closestPlayer = RoomManager.Instance.GetPlayers[i].transform; //change the target to the closest player
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
