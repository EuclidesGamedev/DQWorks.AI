using NUnit.Framework;
using UnityEngine;

namespace DQWorks.Tests.AI.StateMachine
{
    public class StateMachineTests
    {
        private DQWorks.AI.StateMachine.StateMachine _stateMachine;
        private StateForTests _state1, _state2;

        #region Setup for tests
        [SetUp]
        public void SetUp()
        {
            _state1 = new StateForTests();
            _state2 = new StateForTests();
            _stateMachine = new();
        }
        #endregion

        #region Test cases
        [TestCase] public void TestEqualityOfStateInstances()
        {
            Assert.AreNotEqual(_state1, _state2);
            Assert.AreEqual(_state1, _state1);
            Assert.AreEqual(_state2, _state2);
        }

        public void TestAssertInitializeMethodInvokesStateEnter() { }
        public void TestAssertStateMachinesInitialStateIsNull() { }
        public void TestAssertTransitionMethodInvokesStateEnter() { }
        public void TestAssertTransitionMethodInvokesStateExit() { }
        public void TestCantInitializeNull() { }
        public void TestCanTransitionToCurrentState() { }
        public void TestCantTransitionToNull() { }
        #endregion
    }
}
