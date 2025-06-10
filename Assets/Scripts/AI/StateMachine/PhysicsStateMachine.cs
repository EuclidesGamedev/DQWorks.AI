using DQWorks.AI.StateMachine.Interfaces;

namespace DQWorks.AI.StateMachine
{
    public class PhysicsStateMachine : StateMachine<IPhysicsState>
    {
        #region MonoBehaviour
        private void FixedUpdate() => CurrentState?.FixedUpdate();
        private void Update() => CurrentState?.Update();
        #endregion
    }
}
