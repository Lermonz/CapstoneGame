using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    BaseState _currentState;
    void Update() {
        _currentState.UpdateState(this);
    }
    public void SetState(BaseState newState) {
        if(_currentState != null) {
            _currentState.ExitState(this);
        }
        _currentState = newState;
        _currentState.EnterState(this);
    }
}
