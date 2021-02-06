using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private Camera mainCamera;

    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 3f;
    
    Vector3 movement;
    Vector3 movementDirection;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        movePlayer();
    }

    private void movePlayer()
    {
        Vector3 inputDirection = CameraDirection(movementDirection);
        movement = inputDirection * movementSpeed * Time.deltaTime;

        playerRigidbody.MovePosition(transform.position + movement);
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
        return Vector3.ProjectOnPlane(direction, Vector3.up);
    }
}
