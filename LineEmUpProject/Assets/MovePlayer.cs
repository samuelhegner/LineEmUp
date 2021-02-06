using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovePlayer : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private Camera mainCamera;

    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 3f;
    
    public Vector3 movement;
    Vector3 movementDirection;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        
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
        var cameraForward = mainCamera.transform.forward;
        var cameraRight = mainCamera.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        Vector3 direction = cameraForward.normalized * movementDirection.z + cameraRight.normalized * movementDirection.x;
        return Vector3.ProjectOnPlane(direction, Vector3.up);
    }
}
