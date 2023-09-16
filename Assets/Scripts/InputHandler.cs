using System;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class InputHandler : MonoBehaviour
{
    private InputSettings _inputSettings;

    public static event Action<Vector3> MovementKeyPressed;
    public static event Action FireKeyPressed;
    public static event Action FireKeyUnpressed;
    public static event Action ReloadKeyPressed;

    public static event Action<float> WeaponPickKeyPressed;

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        _inputSettings.Enable();
    }
    
    private void Initialize()
    {
        _inputSettings = new InputSettings();
        _inputSettings.Player.Movement.performed += OnMovementKeyPressed;
        _inputSettings.Player.Movement.canceled += OnMovementKeyPressed;
        
        _inputSettings.Player.Fire.performed += OnFireKeyPressed;
        _inputSettings.Player.Fire.canceled += OnFireKeyUnpressed;

        _inputSettings.Player.Reload.started += OnReloadKeyPressed;

        _inputSettings.Player.Inventory.started += OnWeaponPickKeyPressed;
    }

    private void OnMovementKeyPressed(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("Movement");
        Vector3 movementDirection = callbackContext.ReadValue<Vector3>();
        MovementKeyPressed?.Invoke(movementDirection);
    }

    private void OnFireKeyPressed(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("Fired");
        FireKeyPressed?.Invoke();
    }

    private void OnFireKeyUnpressed(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("Unfired");
        FireKeyUnpressed?.Invoke();
    }

    private void OnReloadKeyPressed(InputAction.CallbackContext callbackContext)
    {
        ReloadKeyPressed?.Invoke();
    }

    private void OnWeaponPickKeyPressed(InputAction.CallbackContext callbackContext)
    {
        float keyPressed = callbackContext.ReadValue<float>();
        WeaponPickKeyPressed?.Invoke(keyPressed);
    }
}
