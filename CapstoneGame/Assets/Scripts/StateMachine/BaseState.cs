public abstract class BaseState {
    public abstract void EnterState(PlayerStateMachine playerState);
    public abstract void UpdateState(PlayerStateMachine playerState);
    public abstract void ExitState(PlayerStateMachine playerState);
}