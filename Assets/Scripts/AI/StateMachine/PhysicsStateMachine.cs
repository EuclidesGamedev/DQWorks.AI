using DQWorks.AI.StateMachine.Interfaces;
using UnityEngine;

namespace DQWorks.AI.StateMachine
{
    public class PhysicStateMachine : GenericStateMachine<IPhysicsState>
    {
        #region MonoBehaviour
        private void FixedUpdate() => CurrentState?.FixedUpdate();
        private void Update() => CurrentState?.Update();
        #endregion
    }
}
