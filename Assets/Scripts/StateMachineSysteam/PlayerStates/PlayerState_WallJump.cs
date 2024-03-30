using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/WallJump", fileName = "PlayerState_WallJump")]



public class PlayerState_WallJump : PlayerState
{
	private float wallJumpTimer;
	private float posY;
	private float posX;

	public override void Enter()
    {
		//controller.groundDetect.ResetOnWall();
		controller.IsJumping = true;
		float posY = controller.transform.position.y;
		float posX = controller.transform.position.x;
		wallJumpTimer = 0;
		animator.Play("JumpRise");
        Debug.Log("Now in WallJump State");
		controller.rigidbody.gravityScale = 0.1f;
	}
	public override void Exit()
	{
		controller.rigidbody.gravityScale = 1f;
		controller.IsJumping = false;
	}

	public override void LogicUpdate()
    {
		wallJumpTimer += Time.deltaTime;

		if (controller.IsGrounded && input.StopJump)
		{
			stateMachine.SwitchState(typeof(PlayerState_Land));
		}

		if (wallJumpTimer >= controller.walllJumpTime)
		{
			stateMachine.SwitchState(typeof(PlayerState_Jump));
		}

		if (controller.IsFall && wallJumpTimer >= controller.walllJumpTime)
		{
			stateMachine.SwitchState(typeof(PlayerState_Fall));
		}

		if (controller.canDoubleJump && input.Jump)
		{
			stateMachine.SwitchState(typeof(PlayerState_DoubleJump));
		}
	}

    public override void PhysicUpdate()
    {
		if (wallJumpTimer <= controller.walllJumpTime) controller.WallJump();
	}
}

