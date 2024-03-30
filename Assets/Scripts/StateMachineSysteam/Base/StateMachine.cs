using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
	//�Ыت��A��

	IState CurrentState;

    protected Dictionary<System.Type, IState> stateTable;

	private void OnGUI()
	{
        //Debug.Log(CurrentState);
	}

	//���A��s
	private void Update()
    {
        CurrentState.LogicUpdate();
    }

    //���z��s
    private void FixedUpdate()
    {
        CurrentState.PhysicUpdate();
    }

    //���A����
    protected void SwitchOn(IState newState)
    {
        CurrentState = newState;
        CurrentState.Enter();
    }

    public void SwitchState(IState newState)
    {
        CurrentState.Exit();
        SwitchOn(newState);
    }

    public void SwitchState(System.Type stateType)
    {
        SwitchState(stateTable[stateType]);
    }

}
