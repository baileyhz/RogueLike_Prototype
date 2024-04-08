using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/OnWall", fileName = "PlayerState_OnWall")]



public class PlayerState_WallClimb : PlayerState
{
    private float hlodMove;
    public override void Enter()
    {
		controller.rigidbody.gravityScale = 0f;
        animator.Play("OnWall");
        Debug.Log("Now in OnWall State");
    }
	public override void Exit()
	{
		controller.rigidbody.gravityScale = 1f;
	}
	public override void LogicUpdate()
    {
		if (!controller.IsLeftWall && !controller.IsRightWall)
		{
			stateMachine.SwitchState(typeof(PlayerState_Fall));
		}

		if ((controller.IsLeftWall && input.axes > 0) || (controller.IsRightWall && input.axes < 0)) 
		{
			hlodMove += Time.deltaTime;
			if(hlodMove>=controller.wallCancel)
			{
				controller.groundDetect.ResetOnWall();
				stateMachine.SwitchState(typeof(PlayerState_Fall));
			}
			
		}else
		{
			hlodMove = 0f;
		}

		if (input.Jump && (controller.IsLeftWall || controller.IsRightWall))
		{
			stateMachine.SwitchState(typeof(PlayerState_WallJump));
		}
		
		if (controller.IsGrounded)
        {
			stateMachine.SwitchState(typeof(PlayerState_Land));
		}
	}

    public override void PhysicUpdate()
    {
		controller.Climb();
    }
}

