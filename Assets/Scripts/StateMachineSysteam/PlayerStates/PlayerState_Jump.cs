using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Jump", fileName = "PlayerState_Jump")]

public class PlayerState_Jump : PlayerState
{
    private float jumpYPos;
    
	public override void Enter()
    {
		controller.Jump();
		controller.IsJumping = true;
        animator.Play("JumpRise");
        Debug.Log("Now in Jump State");
	}
	public override void Exit()
	{
		controller.IsJumping = false;
	}

	public override void LogicUpdate()
    {
		if (controller.weapon.IsHide) controller.weapon.WeaponReset();

		if (((controller.IsLeftWall && input.axes < 0) || 
            (controller.IsRightWall && input.axes > 0)) && 
            animator.GetCurrentAnimatorStateInfo(0).IsName("JumpMid"))
        {
            stateMachine.SwitchState(typeof(PlayerState_WallClimb));
        }
        if (controller.canDoubleJump && input.Jump)
        {
            stateMachine.SwitchState(typeof(PlayerState_DoubleJump));
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
		

		if (controller.rigidbody.velocity.y <= controller.jumpStartSpeed / 3f) animator.Play("JumpMid");

		controller.Move();
    }
}
