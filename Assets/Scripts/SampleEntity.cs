using DQWorks.AI.StateMachine;
using DQWorks.AI.StateMachine.Interfaces;
using UnityEngine;

public class SampleEntity : MonoBehaviour
{
    private void Awake()
    {
        var fsm = GetComponent<GenericStateMachine<ISimpleState>>();
        fsm.Initialize(new SampleState());
    }
}
