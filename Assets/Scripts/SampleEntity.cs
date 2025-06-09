using DQWorks.AI.StateMachine;
using DQWorks.AI.StateMachine.Interfaces;
using UnityEngine;

public class SampleEntity : MonoBehaviour
{
    private void Awake()
    {
        var fsm = GetComponent<StateMachine<IState>>();
        fsm.Initialize(new SampleState());
    }
}
