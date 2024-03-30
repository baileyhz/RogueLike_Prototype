using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;


[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Throw", fileName = "PlayerState_Throw")]

public class PlayerState_Throw : PlayerState
{
	public override void Enter()
	{
		animator.Play("Throw");
		controller.ThrowThrowable();
		Debug.Log("Now in Throw State");
	}
	public override void Exit()
	{
	}
	public override void LogicUpdate()
	{
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("Throw") && IsAinmationFinished)
		{
			if (!input.Move)
			{
				stateMachine.SwitchState(typeof(PlayerState_Idle));
			}
			if (controller.IsFall)
			{
				stateMachine.SwitchState(typeof(PlayerState_Fall));
			}
		}
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("Throw") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
		{
			if (input.Dash)
			{
				stateMachine.SwitchState(typeof(PlayerState_Dash));
			}
			if (input.Move)
			{
				stateMachine.SwitchState(typeof(PlayerState_Run));
			}
			if (input.Jump)
			{
				stateMachine.SwitchState(typeof(PlayerState_Jump));
			}
		}
	}

	public override void PhysicUpdate()
	{
		controller.Decelerate();
	}
}
