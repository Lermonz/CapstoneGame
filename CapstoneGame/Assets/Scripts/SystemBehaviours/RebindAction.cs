using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class RebindAction : MonoBehaviour
{
    [SerializeField] private InputActionReference _inputAction;
    [SerializeField] private string _bindingId;
    [SerializeField] private string _bindingText;
    private InputActionRebindingExtensions.RebindingOperation _rebindOperation;

    public void StartRebindOperation()
    {
        if (!ResolveActionAndBinding(out var action, out var bindingIndex))
            return;
        
        _inputAction.action.Disable();
        _rebindOperation = _inputAction.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation =>
            {
                _inputAction.action.Enable();
                RebindCompleted();
            });
        _bindingText = "...";
        _rebindOperation.Start();
    }
    void RebindCompleted()
    {
        _rebindOperation.Dispose();
        UpdateBindingDisplay();
    }
    void UpdateBindingDisplay()
    {
        _bindingText = _inputAction.action.GetBindingDisplayString();
    }
    public bool ResolveActionAndBinding(out InputAction action, out int bindingIndex)
    {
        bindingIndex = -1;

        action = _inputAction?.action;
        if (action == null)
            return false;

        if (string.IsNullOrEmpty(_bindingId))
            return false;

        // Look up binding index.
        var bindingId = new Guid(_bindingId);
        bindingIndex = action.bindings.IndexOf(x => x.id == bindingId);
        if (bindingIndex == -1)
        {
            Debug.LogError($"Cannot find binding with ID '{bindingId}' on '{action}'", this);
            return false;
        }

        return true;
    }
}
