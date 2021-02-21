using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
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
            mousePosition = value.ReadValue<Vector2>();
        }
    }

    void SetMouseAimDirection()
    {
        Vector3 inputIn3D = new Vector3(mousePosition.x, mousePosition.y, mainCamera.transform.position.y);
        Vector3 worldSpaceMousePos = mainCamera.ScreenToWorldPoint(inputIn3D);
        worldSpaceMousePos.y = transform.position.y;
        Vector3 aimDirection = worldSpaceMousePos - transform.position;
        rawInputAim = aimDirection.normalized;
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
        if (currentCotrolScheme != "Gamepad")
        {
            SetMouseAimDirection();
        }
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


    private void OnDestroy()
    {
        RoomManager.Instance.RemovePlayer(transform);
    }
}
