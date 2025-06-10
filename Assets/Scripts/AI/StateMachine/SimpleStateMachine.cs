using DQWorks.AI.StateMachine.Interfaces;
using UnityEngine;

namespace DQWorks.AI.StateMachine
{
    public class SimpleStateMachine : GenericStateMachine<ISimpleState>
    {
        #region MonoBehaviour
        private void Update() => CurrentState?.Update();
        #endregion
    }
}
