using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Attack", fileName = "PlayerState_Attack")]

public class PlayerState_Attack : PlayerState
{
	public override void Enter()
	{
		controller.weapon.back = true;
		animator.Play("Break");
		Debug.Log("Now in Attack State");
	}
	public override void Exit()
	{
		//controller.weapon.back = false;
	}
	public override void LogicUpdate()
	{
		controller.weapon.back = true;
		if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_1") && controller.weapon.IsHide)
		{
			animator.Play("Attack_1");
		}
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_1") && controller.canCombo && input.LightAttack)
		{
			stateMachine.SwitchState(typeof(PlayerState_Attack2));
		}
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_1") && IsAinmationFinished)
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
