using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 3f;

    Vector3 movement;
    Vector3 movementDirection;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        movePlayer();
    }

    /// <summary>
    /// move the player controller based on input, in relation to the camera direction
    /// </summary>
    private void movePlayer()
    {
        Vector3 inputDirection = CameraDirection(movementDirection);
        movement = inputDirection * movementSpeed * Time.fixedDeltaTime;
        if(BoundaryChecker.withinPlayerEdges(transform.position + movement))
            transform.position = transform.position + movement;
    }

    /// <summary>
    /// Set the input direction based on the camera's facing direction
    /// </summary>
    /// <param name="movementDirection">The movement input</param>
    /// <returns></returns>
    Vector3 CameraDirection(Vector3 movementDirection)
    {
        var cameraUp = mainCamera.transform.up;
        var cameraRight = mainCamera.transform.right;

        Vector3 direction = cameraUp * movementDirection.z + cameraRight * movementDirection.x;
        return direction;
    }

    /// <summary>
    /// Updates the players input
    /// </summary>
    /// <param name="smoothMovementInput"></param>
    public void updateMovementData(Vector3 smoothMovementInput)
    {
        movementDirection = smoothMovementInput;
    }
}
