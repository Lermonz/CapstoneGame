using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public static PlayerInput PlayerInput;

    public bool _freezeVelocity = false;

    // Input Properties //
    public float HorizontalInput {get; private set;}
    public bool JumpInput {get; private set;}
    public bool JumpRelease {get; private set;}
    public bool DownInput {get; private set;}
    public bool BoostInput {get; private set;}
    public bool SpinInput {get; private set;}
    public bool MenuOpenInput {get; private set;}

    public bool UIMenuCloseInput {get; private set;}

    private InputAction _horizontalAction;
    private InputAction _jumpAction;
    private InputAction _downAction;
    private InputAction _boostAction;
    private InputAction _spinAction;
    private InputAction _menuOpenAction;

    private InputAction _menuCloseAction;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
        PlayerInput = GetComponent<PlayerInput>();
        _horizontalAction = PlayerInput.actions["Horizontal"];
        _jumpAction = PlayerInput.actions["Jump"];
        _downAction = PlayerInput.actions["Down"];
        _boostAction = PlayerInput.actions["Boost"];
        _spinAction = PlayerInput.actions["Spin"];
        _menuOpenAction = PlayerInput.actions["MenuOpen"];

        _menuCloseAction = PlayerInput.actions["MenuClose"];
    }
    void Update()
    {
        HorizontalInput = _horizontalAction.ReadValue<float>();
        JumpInput = _jumpAction.WasPressedThisFrame();
        JumpRelease = _jumpAction.WasReleasedThisFrame();
        DownInput = _downAction.WasPressedThisFrame();
        BoostInput = _boostAction.WasPressedThisFrame();
        SpinInput = _spinAction.WasPressedThisFrame();
        MenuOpenInput = _menuOpenAction.WasPressedThisFrame();
        UIMenuCloseInput = _menuCloseAction.WasPressedThisFrame();
    }
    public void DisablePlayerInput() {
        PlayerInput.SwitchCurrentActionMap("UI");
    }
    public void EnablePlayerInput() {
        PlayerInput.SwitchCurrentActionMap("Player");
    }
    public void NegateAllInput() {
        PlayerInput.SwitchCurrentActionMap("None");
    }
}
