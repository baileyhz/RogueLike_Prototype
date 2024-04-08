using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Fall", fileName = "PlayerState_Fall")]
public class PlayerState_Fall : PlayerState
{
	
	public override void Enter()
    {
        animator.Play("JumpFall");
        controller.rigidbody.gravityScale = 2f;
        Debug.Log("Now in Fall State");
    }

	public override void Exit()
	{
		controller.rigidbody.gravityScale = 1f;
	}

	public override void LogicUpdate()
    {
		if (!controller.canDoubleJump && input.Jump)
		{
			input.SetJumpPreInput();
		}

		if ((controller.IsLeftWall) || (controller.IsRightWall))
		{
			stateMachine.SwitchState(typeof(PlayerState_WallClimb));
		}

		if (controller.canDoubleJump && input.Jump)
		{
			stateMachine.SwitchState(typeof(PlayerState_DoubleJump));
		}

		if (controller.IsGrounded)
        {
            stateMachine.SwitchState(typeof(PlayerState_Land));
        }
		if (input.HoldThrow)
		{
			stateMachine.SwitchState(typeof(PlayerState_ThrowAim));
		}
		if (input.PressThrow)
		{
			stateMachine.SwitchState(typeof(PlayerState_Throw));
		}
	}

    public override void PhysicUpdate()
    {
        controller.Fall();
		controller.Move();
	}
}
