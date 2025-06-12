using DQWorks.AI.StateMachine.Interfaces;
using UnityEngine;

namespace DQWorks.AI.StateMachine
{
    // Abstract template class for state machines
    public abstract class StateMachine<TState> : MonoBehaviour where TState : IState
    {
        #region Getters and setters
        public TState CurrentState { get; private set; }
        #endregion

        #region StateMachine
        public void Initialize(TState state)
        {
            if (state == null)
                throw new System.ArgumentNullException();
            if (CurrentState != null)
                return;
            CurrentState = state;
            CurrentState.Enter();
        }

        public void Transition(TState newState)
        {
            if (newState == null)
                throw new System.ArgumentNullException();
            if (CurrentState != null)
                CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
        #endregion
    }

    // Default implementation of StateMachine
    public class StateMachine : StateMachine<IState> { }
}
