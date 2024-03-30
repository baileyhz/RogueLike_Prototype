using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Land", fileName = "PlayerState_Land")]
public class PlayerState_Land : PlayerState
{
    public override void Enter()
    {
        animator.Play("Land");
        Debug.Log("Now in Land State");
        controller.rigidbody.gravityScale = 1.0f;
		controller.doubleJumpCount = 0;
		if (input.preJumpInput)
		{
			if (controller.weapon.IsHide) controller.weapon.WeaponReset();
			stateMachine.SwitchState(typeof(PlayerState_Jump));
		}
	}

	public override void Exit()
	{
		input.preJumpInput = false;
	}

	public override void LogicUpdate()
    {
		if (!input.Move)
        {
			controller.weapon.back = true;
			if (controller.weapon.IsHold)
			{
				stateMachine.SwitchState(typeof(PlayerState_Idle));
			}
		}
		if (input.Dash && controller.canDash)
		{
			stateMachine.SwitchState(typeof(PlayerState_Dash));
		}
		if (input.Move)
		{
			controller.weapon.back = false;
			stateMachine.SwitchState(typeof(PlayerState_Run));
		}
		if (input.Jump && !controller.IsTop)
		{
			if (controller.weapon.IsHide) controller.weapon.WeaponReset();
			stateMachine.SwitchState(typeof(PlayerState_Jump));
		}

	}
	public override void PhysicUpdate()
	{
		controller.Move();
	}
}
