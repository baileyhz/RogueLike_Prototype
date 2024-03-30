using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
	//創建狀態機

	IState CurrentState;

    protected Dictionary<System.Type, IState> stateTable;

	private void OnGUI()
	{
        //Debug.Log(CurrentState);
	}

	//狀態更新
	private void Update()
    {
        CurrentState.LogicUpdate();
    }

    //物理更新
    private void FixedUpdate()
    {
        CurrentState.PhysicUpdate();
    }

    //狀態切換
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
