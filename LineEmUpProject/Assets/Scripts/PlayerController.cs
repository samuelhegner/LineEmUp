using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerAiming playerAiming;
    [SerializeField] Camera mainCamera;


    [Header("Player Input Settings")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float movementSmoothingSpeed = 1f;
    [SerializeField] private float aimSmoothingSpeed = 1f;

    private Vector3 rawInputMovement;
    private Vector3 smoothInputMovement;
    private Vector3 rawInputAim;
    private Vector3 smoothInputAim;

    public event Action chargeDown;
    public event Action chargeRelease;


    [SerializeField] private string currentCotrolScheme;
    private void Awake()
    {
        OnControlSchemeChanged();
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();
        rawInputMovement = new Vector3(inputMovement.x, 0, inputMovement.y);
    }

    public void OnAim(InputAction.CallbackContext value) 
    {
        if (currentCotrolScheme == "Gamepad")
        {
            Vector2 inputAim = value.ReadValue<Vector2>();
            rawInputAim = new Vector3(inputAim.x, 0, inputAim.y);
        }
        else 
        {
            Vector2 inputAim = value.ReadValue<Vector2>();
            Vector3 inputIn3D = new Vector3(inputAim.x, inputAim.y, mainCamera.transform.position.y);
            Vector3 worldSpaceMousePos = mainCamera.ScreenToWorldPoint(inputIn3D);
            worldSpaceMousePos.y = transform.position.y;
            Vector3 aimDirection = worldSpaceMousePos - transform.position;
            
            rawInputAim = aimDirection.normalized;
        }
        
    }

    public void OnCharge(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            chargeDown?.Invoke();
        }
        else if (value.canceled) 
        {
            chargeRelease?.Invoke();
        }
    }

    public void OnControlSchemeChanged() 
    {
        currentCotrolScheme = playerInput.currentControlScheme;
    }

    void Update()
    {
        CalculateMovementInputSmoothing();
        CalculateAimInputSmoothing();
        UpdatePlayerMovement();
        UpdatePlayerAim();
    }

    

    private void CalculateMovementInputSmoothing()
    {
        smoothInputMovement = Vector3.Lerp(smoothInputMovement, rawInputMovement, Time.deltaTime * movementSmoothingSpeed);
    }

    private void CalculateAimInputSmoothing() 
    {
        smoothInputAim = Vector3.Lerp(smoothInputAim, rawInputAim, Time.deltaTime * aimSmoothingSpeed);
    }

    void UpdatePlayerMovement()
    {
        playerMovement.updateMovementData(smoothInputMovement);
    }
    private void UpdatePlayerAim()
    {
        playerAiming.updateAimingData(smoothInputAim);
    }

    
    
}
