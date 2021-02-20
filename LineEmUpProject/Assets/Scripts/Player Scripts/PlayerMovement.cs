using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private Camera mainCamera;

    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 3f;

    Vector3 movement;
    Vector3 movementDirection;



    void Update()
    {
        movePlayer();
    }

    private void movePlayer()
    {
        Vector3 inputDirection = CameraDirection(movementDirection);
        movement = inputDirection * movementSpeed * Time.fixedDeltaTime;
        if(BoundaryChecker.withinPlayerEdges(transform.position + movement))
            transform.position = transform.position + movement;
    }

    public void updateMovementData(Vector3 smoothMovementInput)
    {
        movementDirection = smoothMovementInput;
    }

    Vector3 CameraDirection(Vector3 movementDirection)
    {
        var cameraUp = mainCamera.transform.up;
        var cameraRight = mainCamera.transform.right;

        Vector3 direction = cameraUp * movementDirection.z + cameraRight * movementDirection.x;
        return direction;
    }
}
