using System;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class InputHandler : MonoBehaviour
{
    private InputSettings _inputSettings;

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

        _inputSettings.Player.SecondaryFire.performed += OnSecondaryFireKeyPressed;
        _inputSettings.Player.SecondaryFire.canceled += OnSecondaryFireKeyUnpressed;

        _inputSettings.Player.Reload.started += OnReloadKeyPressed;

        _inputSettings.Player.Inventory.started += OnWeaponPickKeyPressed;
    }

    private void OnMovementKeyPressed(InputAction.CallbackContext callbackContext)
    {
        Vector3 movementDirection = callbackContext.ReadValue<Vector3>();
        EventManager.RaiseEvent(new MovementKeyPressedEventArgs(movementDirection));
    }

    private void OnSecondaryFireKeyPressed(InputAction.CallbackContext callbackContext)
    {
        EventManager.RaiseEvent(new SecondaryFireKeyPressedEventArgs());
    }

    private void OnSecondaryFireKeyUnpressed(InputAction.CallbackContext callbackContext)
    {
        EventManager.RaiseEvent(new SecondaryFireKeyUnpressedEventArgs());
    }

    private void OnFireKeyPressed(InputAction.CallbackContext callbackContext)
    {
        EventManager.RaiseEvent(new FireKeyPressedEventArgs());
    }

    private void OnFireKeyUnpressed(InputAction.CallbackContext callbackContext)
    {
        EventManager.RaiseEvent(new FireKeyUnpressedEventArgs());
    }

    private void OnReloadKeyPressed(InputAction.CallbackContext callbackContext)
    {
        EventManager.RaiseEvent(new ReloadKeyPressedEventArgs());
    }

    private void OnWeaponPickKeyPressed(InputAction.CallbackContext callbackContext)
    {
        float keyPressed = callbackContext.ReadValue<float>();
        EventManager.RaiseEvent(new ItemSelectKeyPressedEventArgs(keyPressed));
    }
}
