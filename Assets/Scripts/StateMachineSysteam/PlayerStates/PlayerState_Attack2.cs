using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Attack2", fileName = "PlayerState_Attack2")]

public class PlayerState_Attack2 : PlayerState
{
	public override void Enter()
	{
		controller.weapon.back = true;
		Debug.Log("Now in Attack2 State");
		animator.Play("Attack_2");
	}
	public override void Exit()
	{
		//controller.weapon.back = false;
	}
	public override void LogicUpdate()
	{
		controller.weapon.back = true;
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_2") && IsAinmationFinished)
		{
			if (input.Move)
			{
				stateMachine.SwitchState(typeof(PlayerState_Run));
			}
			if (!input.Move)
			{
				stateMachine.SwitchState(typeof(PlayerState_Idle));
			}
			if (input.Dash)
			{
				stateMachine.SwitchState(typeof(PlayerState_Dash));
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
