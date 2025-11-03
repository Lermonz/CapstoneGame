using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour, IDataPersistence
{
    public static InputManager Instance;
    public static PlayerInput PlayerInput;

    public bool _freezeVelocity = false;

    // Input Properties //
    public Vector2 Dpad { get; private set; }
    public float HorizontalInput { get; private set; }
    public bool JumpInput { get; private set; }
    public bool JumpRelease { get; private set; }
    public bool DownInput { get; private set; }
    public bool BoostInput { get; private set; }
    public bool SpinInput { get; private set; }
    public bool MenuOpenInput { get; private set; }
    public bool QuickResetInput { get; private set; }

    public bool UIMenuCloseInput { get; private set; }
    public bool UIConfirm { get; private set; }
    public bool UICancel { get; private set; }
    public Vector2 MouseScrollInput { get; private set; }

    private InputAction _dpadAction;
    private InputAction _horizontalAction;
    private InputAction _jumpAction;
    private InputAction _downAction;
    private InputAction _boostAction;
    private InputAction _spinAction;
    private InputAction _menuOpenAction;
    private InputAction _quickResetAction;

    private InputAction _menuCloseAction;
    private InputAction _uiMove;
    private InputAction _uiConfirm;
    private InputAction _uiCancel;
    private InputAction _uiMouseScroll;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
        PlayerInput = GetComponent<PlayerInput>();
        _dpadAction = PlayerInput.actions["Movement"];
        _horizontalAction = PlayerInput.actions["Horizontal"];
        _jumpAction = PlayerInput.actions["Jump"];
        _downAction = PlayerInput.actions["Down"];
        _boostAction = PlayerInput.actions["Boost"];
        _spinAction = PlayerInput.actions["Spin"];
        _menuOpenAction = PlayerInput.actions["MenuOpen"];
        _quickResetAction = PlayerInput.actions["QuickReset"];
        _menuCloseAction = PlayerInput.actions["MenuClose"];
        _uiConfirm = PlayerInput.actions["UI_Confirm"];
        _uiCancel = PlayerInput.actions["UI_Cancel"];
        _uiMouseScroll = PlayerInput.actions["UI_MouseScroll"];
    }
    void Update()
    {
        Dpad = _dpadAction.ReadValue<Vector2>();
        HorizontalInput = _horizontalAction.ReadValue<float>();
        JumpInput = _jumpAction.WasPressedThisFrame();
        JumpRelease = _jumpAction.WasReleasedThisFrame();
        DownInput = _downAction.WasPressedThisFrame();
        BoostInput = _boostAction.WasPressedThisFrame();
        SpinInput = _spinAction.WasPressedThisFrame();
        MenuOpenInput = _menuOpenAction.WasPressedThisFrame();
        QuickResetInput = _quickResetAction.WasPressedThisFrame();
        UIMenuCloseInput = _menuCloseAction.WasPressedThisFrame();
        UIConfirm = _uiConfirm.WasPressedThisFrame();
        UICancel = _uiCancel.WasPressedThisFrame();
        MouseScrollInput = _uiMouseScroll.ReadValue<Vector2>();
    }
    public void DisablePlayerInput()
    {
        PlayerInput.SwitchCurrentActionMap("UI");
    }
    public void EnablePlayerInput()
    {
        PlayerInput.SwitchCurrentActionMap("Player");
    }
    public void NegateAllInput()
    {
        PlayerInput.SwitchCurrentActionMap("None");
        Debug.Log("input negated");
    }
    public void SaveData(GameData data)
    {
        data.rebinds = PlayerInput.actions.SaveBindingOverridesAsJson();
    }
    public void LoadData(GameData data)
    {
        PlayerInput.actions.LoadBindingOverridesFromJson(data.rebinds);
    }
    public string GetControlScheme()
    {
        return PlayerInput.currentControlScheme;
    }
}
