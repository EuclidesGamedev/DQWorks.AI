using DQWorks.AI.StateMachine;
using NUnit.Framework;

namespace DQWorks.Tests.AI.StateMachine
{
    public class PhysicsStateMachineTests
    {
        private PhysicsStateMachine _stateMachine;
        private PhysicsStateForTests _state1, _state2;

        #region Setup for StateMachine tests
        [SetUp]
        public void SetUp()
        {
            _state1 = new PhysicsStateForTests();
            _state2 = new PhysicsStateForTests();
            _stateMachine = new();
        }
        #endregion

        #region StateMachine tests
        [TestCase]
        public void TestEqualityOfStateInstances()
        {
            Assert.AreNotEqual(_state1, _state2);
            Assert.AreEqual(_state1, _state1);
            Assert.AreEqual(_state2, _state2);
        }

        [TestCase]
        public void TestAssertInitializeMethodInvokesStateEnter()
        {
            // Setup for the test
            bool _calledEnter = false;
            _state1.OnStateEnter += () => _calledEnter = true;

            // Pre-assertions
            Assert.False(_calledEnter);

            // Execute logic
            _stateMachine.Initialize(_state1);

            // Post-assertions
            Assert.AreEqual(_state1, _stateMachine.CurrentState);
            Assert.True(_calledEnter);
        }

        [TestCase]
        public void TestAssertStateMachinesInitialStateIsNull()
        {
            // Assert initial state is null
            Assert.Null(_stateMachine.CurrentState);
        }

        [TestCase]
        public void TestAssertTransitionChangesCurrentState()
        {
            // Setup for the test
            _stateMachine.Initialize(_state1);

            // Pre-assertions
            Assert.AreEqual(_state1, _stateMachine.CurrentState);

            // Execute logic
            _stateMachine.Transition(_state2);

            // Post-assertions
            Assert.AreEqual(_state2, _stateMachine.CurrentState);
        }

        [TestCase]
        public void TestAssertTransitionMethodInvokesStateEnter()
        {
            // Setup for the test
            bool _calledEnter = false;
            _state2.OnStateEnter += () => _calledEnter = true;
            _stateMachine.Initialize(_state1);

            // Pre-assertions
            Assert.False(_calledEnter);

            // Execute logic
            _stateMachine.Transition(_state2);

            // Post-assertions
            Assert.AreEqual(_state2, _stateMachine.CurrentState);
            Assert.True(_calledEnter);
        }

        [TestCase]
        public void TestAssertTransitionMethodInvokesStateExit()
        {
            // Setup for the test
            bool _calledExit = false;
            _state1.OnStateExit += () => _calledExit = true;
            _stateMachine.Initialize(_state1);

            // Pre-assertions
            Assert.False(_calledExit);

            // Execute logic
            _stateMachine.Transition(_state2);

            // Post-assertions
            Assert.AreEqual(_state2, _stateMachine.CurrentState);
            Assert.True(_calledExit);
        }

        [TestCase]
        public void TestCantInitializeNull()
        {
            // Assert initialize null state throws an exception
            Assert.Throws<System.ArgumentNullException>(
                () => _stateMachine.Initialize(null)
            );
        }

        [TestCase]
        public void TestCanTransitionToCurrentState()
        {
            // Setup for the test
            bool _calledExit = false;
            _state1.OnStateExit += () => _calledExit = true;
            _stateMachine.Initialize(_state1);

            // Pre-assertions
            Assert.AreEqual(_state1, _stateMachine.CurrentState);
            Assert.False(_calledExit);

            // Execute logic
            _stateMachine.Transition(_state1);

            // Post-assertions
            Assert.AreEqual(_state1, _stateMachine.CurrentState);
            Assert.True(_calledExit);
        }

        [TestCase]
        public void TestCantTransitionToNull()
        {
            // Assert transition to null state throws an exception
            Assert.Throws<System.ArgumentNullException>(
                () => _stateMachine.Initialize(null)
            );
        }
        #endregion
    }
}
