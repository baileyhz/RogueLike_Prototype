using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/OnHit", fileName = "PlayerState_OnHit")]



public class PlayerState_OnHit : PlayerState
{
    public override void Enter()
    {
		if (character.IsHitRecover)
		{
			animator.Play("Idle");
		}
       
		Debug.Log("Now in onhit State");
		
		foreach (Bless b in character.blesses)
		{
			b.OnHit(controller, character);
		}
	}

	public override void Exit()
	{
        controller.IsHit = false;
	}

	public override void LogicUpdate()
    {
		if (controller.IsHit)
		{
			stateMachine.SwitchState(typeof(PlayerState_OnHit));
		}
        if (input.Move && !character.IsHitRecover)
        {
            stateMachine.SwitchState(typeof(PlayerState_Run));
        }
		if (!input.Move && !character.IsHitRecover)
		{
			stateMachine.SwitchState(typeof(PlayerState_Idle));
		}
		if (input.Jump && controller.IsGrounded && !controller.IsTop && !character.IsHitRecover)
        {
            stateMachine.SwitchState(typeof(PlayerState_Jump));
        }
        if (!controller.IsGrounded && !character.IsHitRecover)
        {
            stateMachine.SwitchState(typeof(PlayerState_Fall));
        }
		if (input.Dash && controller.canDash && !character.IsHitRecover)
		{
			stateMachine.SwitchState(typeof(PlayerState_Dash));
		}
	}

    public override void PhysicUpdate()
    {
        controller.Move();
    }
}

