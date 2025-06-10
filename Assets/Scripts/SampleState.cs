using DQWorks.AI.StateMachine.Interfaces;
using UnityEngine;

public class SampleState : ISimpleState
{
    public void Enter() { Debug.Log("Joined in SampleState"); }
    public void Exit() { Debug.Log("Quited in SampleState"); }
    public void FixedUpdate() { Debug.Log("FixedUpdated SampleState"); }
    public void Update() { Debug.Log("Updated SampleState"); }
}
