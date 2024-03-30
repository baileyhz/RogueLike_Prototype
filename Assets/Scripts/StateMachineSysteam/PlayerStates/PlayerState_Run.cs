using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;



[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Run", fileName = "PlayerState_Run")]

public class PlayerState_Run : PlayerState
{
    public override void Enter()
    {
		if(controller.weapon.IsHide) controller.weapon.WeaponReset();
        animator.Play("Run");
        Debug.Log("Now in Run State");
    }
	public override void Exit()
	{
		controller.IsTurning = false;
	}

	public override void LogicUpdate()
    {
		if (input.Move && animator.GetCurrentAnimatorStateInfo(0).IsName("Break")) 
		{
			animator.Play("Run");
			controller.weapon.back = false;
			if (controller.weapon.IsHide) controller.weapon.WeaponReset();
		} 

		if (IsAinmationFinished && animator.GetCurrentAnimatorStateInfo(0).IsName("Turn") && !controller.IsTurning) 
			animator.Play("Run");

		if (controller.IsTurning && !(controller.IsLeftWall || controller.IsRightWall)) 
		{ 
			if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Turn")) animator.Play("Turn");
		}

		#region SwitchState
		if (!input.Move)
		{
			animator.Play("Break");
			controller.weapon.back = true;

			if (controller.weapon.IsHold) //(IsAinmationFinished && animator.GetCurrentAnimatorStateInfo(0).IsName("Break")) || 
			{
				stateMachine.SwitchState(typeof(PlayerState_Idle));
			}
		}
		if(input.LightAttack)
		{
			stateMachine.SwitchState(typeof(PlayerState_Attack));
		}
		
		if (input.Jump && controller.IsGrounded && !controller.IsTop)
		{
			stateMachine.SwitchState(typeof(PlayerState_Jump));
		}

		if (!controller.IsGrounded)
		{
			stateMachine.SwitchState(typeof(PlayerState_Fall));
		}

		if (input.Dash && controller.canDash)
		{
			stateMachine.SwitchState(typeof(PlayerState_Dash));
		}
		if (input.HoldThrow)
		{
			stateMachine.SwitchState(typeof(PlayerState_ThrowAim));
		}
		if (input.PressThrow)
		{
			stateMachine.SwitchState(typeof(PlayerState_Throw));
		}
		#endregion
	}

	public override void PhysicUpdate()
    {
		controller.Move();
	}
}


