using DQWorks.AI.StateMachine.Interfaces;
using UnityEngine;

namespace DQWorks.AI.StateMachine
{
    public abstract class GenericStateMachine<TState> : MonoBehaviour where TState: IStaticState
    {
        #region Getters and setters
        public TState CurrentState { get; private set; }
        #endregion

        #region StateMachine
        public void Initialize(TState state)
        {
            if (CurrentState != null)
                return;
            CurrentState = state;
            CurrentState.Enter();
        }

        public void Transition(TState newState)
        {
            if (CurrentState != null)
                CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
        #endregion
    }
}
