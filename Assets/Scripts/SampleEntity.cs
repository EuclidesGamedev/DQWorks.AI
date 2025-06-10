using DQWorks.AI.StateMachine;
using UnityEngine;

public class SampleEntity : MonoBehaviour
{
    private void Awake()
    {
        var fsm = GetComponent<PhysicsStateMachine>();
        fsm.Initialize(new SampleState());
    }
}
