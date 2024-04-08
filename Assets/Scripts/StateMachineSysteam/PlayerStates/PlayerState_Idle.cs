using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Idle", fileName = "PlayerState_Idle")]



public class PlayerState_Idle : PlayerState
{
    
    public override void Enter()
    {
		controller.rigidbody.velocity = Vector2.zero;
		animator.Play("Idle");
        Debug.Log("Now in Idle State");
    }

    public override void LogicUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) controller.weapon.WeaponHide();
        
        if (input.HeavyAttack)
        {
            Debug.Log("Heavy");
        }
        if (input.LightAttack)
        {
            stateMachine.SwitchState(typeof(PlayerState_Attack));
        }
        if(IsAinmationFinished && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) animator.Play("Idle");

        if (!input.Move && animator.GetCurrentAnimatorStateInfo(0).IsName("IdleToRun"))
        {
            if (!controller.weapon.IsHide) controller.weapon.back = true;
            animator.Play("Idle");
		}

        if (controller.IsHit)
        {
            stateMachine.SwitchState(typeof(PlayerState_OnHit));
        }
        if (input.Move)
        {
		    animator.Play("IdleToRun");
			
            controller.weapon.WeaponReset();
            
            if (IsAinmationFinished)
            {
				stateMachine.SwitchState(typeof(PlayerState_Run)); 
			}
        }
        if (input.Jump && controller.IsGrounded && !controller.IsTop)
        {
			controller.weapon.WeaponReset();
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
	}

    public override void PhysicUpdate()
    {
        controller.Move();
    }
}

