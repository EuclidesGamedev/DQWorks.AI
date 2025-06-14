using DQWorks.AI.StateMachine.Interfaces;

namespace DQWorks.AI.StateMachine
{
    public abstract class SimpleStateMachine<TState> : StateMachine<TState> where TState : ISimpleState
    {
        #region MonoBehaviour
        private void Update() => CurrentState?.Update();
        #endregion
    }

    public class SimpleStateMachine : SimpleStateMachine<ISimpleState> { }
}
