using DQWorks.AI.StateMachine.Interfaces;

namespace DQWorks.AI.StateMachine
{
    public abstract class PhysicsStateMachine<TState> : StateMachine<TState> where TState : IPhysicsState
    {
        #region MonoBehaviour
        private void FixedUpdate() => CurrentState?.FixedUpdate();
        private void Update() => CurrentState?.Update();
        #endregion
    }

    public class PhysicsStateMachine : PhysicsStateMachine<IPhysicsState> { }
}
