using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/DoubleJump", fileName = "PlayerState_DoubleJump")]

public class PlayerState_DoubleJump : PlayerState
{
    private float jumpYPos;

    public override void Enter()
    {
        controller.rigidbody.velocity = new Vector2(controller.rigidbody.velocity.x, 0f);
		controller.Jump();
        controller.doubleJumpCount++;
        controller.IsJumping = true;
        animator.Play("JumpRise");
        Debug.Log("Now in double Jump State");
    }
	public override void Exit()
	{
		controller.IsJumping = false;
	}

	public override void LogicUpdate()
    {
        
        if (input.Jump && controller.canDoubleJump)
        {
            stateMachine.SwitchState(typeof(PlayerState_DoubleJump));
        }
        if (controller.IsGrounded && input.StopJump)
        {
            stateMachine.SwitchState(typeof(PlayerState_Land));
        }
		if (controller.IsFall || controller.IsTop)
        {
			stateMachine.SwitchState(typeof(PlayerState_Fall));
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
		controller.Move();
		if (controller.rigidbody.velocity.y <= controller.jumpStartSpeed/3f) 
            animator.Play("JumpMid");
    }
}
