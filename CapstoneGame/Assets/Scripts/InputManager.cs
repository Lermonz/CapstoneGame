using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    // Input Properties //
    public float HorizontalInput {get; private set;}
    public bool JumpInput {get; private set;}
    public bool DownInput {get; private set;}
    public bool BoostInput {get; private set;}
    public bool SpinInput {get; private set;}
    public bool MenuOpenCloseInput {get; private set;}

    PlayerInput _playerInput;

    private InputAction _horizontalAction;
    private InputAction _jumpAction;
    private InputAction _downAction;
    private InputAction _boostAction;
    private InputAction _spinAction;
    private InputAction _menuOpenCloseAction;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
        _playerInput = GetComponent<PlayerInput>();
        _horizontalAction = _playerInput.actions["Horizontal"];
        _jumpAction = _playerInput.actions["Jump"];
        _downAction = _playerInput.actions["Down"];
        _boostAction = _playerInput.actions["Boost"];
        _spinAction = _playerInput.actions["Spin"];
        _menuOpenCloseAction = _playerInput.actions["MenuOpenClose"];
    }
    void Update()
    {
        HorizontalInput = _horizontalAction.ReadValue<float>();
        JumpInput = _jumpAction.WasPressedThisFrame();
        DownInput = _downAction.WasPressedThisFrame();
        BoostInput = _boostAction.WasPressedThisFrame();
        SpinInput = _spinAction.WasPressedThisFrame();
        MenuOpenCloseInput = _menuOpenCloseAction.WasPressedThisFrame();
    }
}
