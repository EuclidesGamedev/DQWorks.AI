using DQWorks.AI.StateMachine.Interfaces;
using UnityEngine.Events;

namespace DQWorks.Tests.AI.StateMachine
{
    public class StateForTests : IState
    {
        public UnityAction OnStateEnter;
        public UnityAction OnStateExit;

        public void Enter() { OnStateEnter?.Invoke(); }
        public void Exit() { OnStateExit?.Invoke(); }
    }

    public class SimpleStateForTests : ISimpleState
    {
        public UnityAction OnStateEnter;
        public UnityAction OnStateExit;

        public void Enter() { OnStateEnter?.Invoke(); }
        public void Exit() { OnStateExit?.Invoke(); }
        public void Update() { }
    }

    public class PhysicsStateForTests : IPhysicsState
    {
        public UnityAction OnStateEnter;
        public UnityAction OnStateExit;

        public void Enter() { OnStateEnter?.Invoke(); }
        public void Exit() { OnStateExit?.Invoke(); }
        public void FixedUpdate() { }
        public void Update() { }
    }
}
