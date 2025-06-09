using DQWorks.AI.StateMachine.Interfaces;
using UnityEngine;

namespace DQWorks.AI.StateMachine
{
    public class StateMachine : MonoBehaviour
    {
        #region Getters and setters
        public IState CurrentState { get; private set; }
        #endregion

        #region MonoBehaviour
        private void FixedUpdate() => CurrentState?.FixedUpdate();
        private void Update() => CurrentState?.Update();
        #endregion

        #region StateMachine
        public void Initialize(IState state)
        {
            if (CurrentState != null)
                return;
            CurrentState = state;
            CurrentState.Enter();
        }

        public void Transition(IState newState)
        {
            if (CurrentState != null)
                CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
        #endregion
    }
}
