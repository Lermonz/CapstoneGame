using System;
using UnityEngine;

public class GlobalOnOffState : MonoBehaviour
{

    public bool SwitchState { get; private set; }
    public Action OnState;
    public Action OffState;
    public static GlobalOnOffState Instance;
    [SerializeField] Player _player;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
            Instance = this;
    }
    void Start()
    {
        _player.SwapState += SwapStatesEvent;
    }
    void OnDestroy()
    {
        _player.SwapState -= SwapStatesEvent;
    }
    void SwapStatesEvent()
    {
        SwitchState = !SwitchState;
        if (SwitchState) { OnState?.Invoke(); }
        else { OffState?.Invoke(); }
    }
}
