using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/ThrowAim", fileName = "PlayerState_ThrowAim")]

public class PlayerState_ThrowAim : PlayerState
{
	public override void Enter()
	{
		if (controller.weapon.IsHide) controller.weapon.WeaponReset();
		animator.Play("Aiming");
		Debug.Log("Now in Throw StateAim");
	}
	public override void Exit()
	{
		controller.throwAngle = 0;
		controller.SetThrowLine(false);
	}
	public override void LogicUpdate()
	{
		if (input.ReleasThrow)
		{
			stateMachine.SwitchState(typeof(PlayerState_Throw));
		}
		if (input.Dash && controller.canDash)
		{
			stateMachine.SwitchState(typeof(PlayerState_Dash));
		}
	}

	public override void PhysicUpdate()
	{
		controller.SetThrowLine(true);
		controller.Turn();
		controller.Decelerate();
	}
}
