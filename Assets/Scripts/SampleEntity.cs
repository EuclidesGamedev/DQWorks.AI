using DQWorks.AI.StateMachine;
using UnityEngine;

public class SampleEntity : MonoBehaviour
{
    private void Awake()
    {
        var fsm = GetComponent<StateMachine>();
        fsm.Initialize(new SampleState());
    }
}
