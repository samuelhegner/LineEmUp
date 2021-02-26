using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerAiming playerAiming;



    [Header("Player Input Settings")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float movementSmoothingSpeed = 1f;
    [SerializeField] private float aimSmoothingSpeed = 1f;
    [SerializeField] private string currentCotrolScheme;


    private Vector3 rawInputMovement;
    private Vector3 smoothInputMovement;
    private Vector3 rawInputAim;
    private Vector3 smoothInputAim;

    public event Action chargeDown;
    public event Action chargeRelease;

    Vector2 mousePosition;

    Camera mainCamera;



    private void Awake()
    {
        mainCamera = Camera.main;
        OnControlSchemeChanged();
        RoomManager.Instance.AddPlayer(transform);
    }

    void Update()
    {
        if (currentCotrolScheme != "Gamepad")
        {
            SetMouseAimDirection();
        }
        CalculateMovementInputSmoothing();
        CalculateAimInputSmoothing();
        UpdatePlayerMovement();
        UpdatePlayerAim();
    }

    /// <summary>
    /// Move action event
    /// </summary>
    /// <param name="value"></param>
    public void OnMove(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();
        rawInputMovement = new Vector3(inputMovement.x, 0, inputMovement.y);
    }

    /// <summary>
    /// Aim action event
    /// </summary>
    /// <param name="value"></param>
    public void OnAim(InputAction.CallbackContext value)
    {
        if (currentCotrolScheme == "Gamepad")
        {
            Vector2 inputAim = value.ReadValue<Vector2>();
            rawInputAim = new Vector3(inputAim.x, 0, inputAim.y);
        }
        else
        {
            mousePosition = value.ReadValue<Vector2>();
        }
    }

    /// <summary>
    /// Set the aim direction for mouse input
    /// </summary>
    void SetMouseAimDirection()
    {
        Vector3 inputIn3D = new Vector3(mousePosition.x, mousePosition.y, mainCamera.transform.position.y);
        Vector3 worldSpaceMousePos = mainCamera.ScreenToWorldPoint(inputIn3D);
        worldSpaceMousePos.y = transform.position.y;
        Vector3 aimDirection = worldSpaceMousePos - transform.position;
        rawInputAim = aimDirection.normalized;
    }

    /// <summary>
    /// Charge action event
    /// </summary>
    /// <param name="value"></param>
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

    /// <summary>
    /// Callback for when control scheme changes
    /// </summary>
    public void OnControlSchemeChanged()
    {
        currentCotrolScheme = playerInput.currentControlScheme;
    }

    /// <summary>
    /// Smooth the raw movement input
    /// </summary>
    private void CalculateMovementInputSmoothing()
    {
        smoothInputMovement = Vector3.Lerp(smoothInputMovement, rawInputMovement, Time.deltaTime * movementSmoothingSpeed);
    }

    /// <summary>
    /// Smooth the raw aiming input
    /// </summary>
    private void CalculateAimInputSmoothing()
    {
        smoothInputAim = Vector3.Lerp(smoothInputAim, rawInputAim, Time.deltaTime * aimSmoothingSpeed);
    }

    /// <summary>
    /// Update the player movement script with new input info
    /// </summary>
    void UpdatePlayerMovement()
    {
        playerMovement.updateMovementData(smoothInputMovement);
    }

    /// <summary>
    /// Update the player aim script with new input info
    /// </summary>
    private void UpdatePlayerAim()
    {
        playerAiming.updateAimingData(smoothInputAim);
    }


    private void OnDestroy()
    {
        RoomManager.Instance.RemovePlayer(transform);
    }
}
