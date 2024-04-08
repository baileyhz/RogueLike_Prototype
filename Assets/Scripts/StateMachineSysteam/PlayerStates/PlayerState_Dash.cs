using UnityEngine;
using UnityEngine.InputSystem;



[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Dash", fileName = "PlayerState_Dash")]

public class PlayerState_Dash : PlayerState
{
	private float dashXPos;
	public override void Enter()
	{
		animator.Play("Dash");
		controller.IsDash = true;
		dashXPos = controller.transform.position.x;
		controller.weapon.WeaponReset();
	}

	public override void Exit() 
	{
		if (controller.transform.localScale.x > 0)
		{
			controller.rigidbody.velocity = new Vector2(controller.maxSpeed, 0);
		}
		else
		{
			controller.rigidbody.velocity = new Vector2(-controller.maxSpeed, 0);
		}
		controller.IsDash = false;
	}

	public override void LogicUpdate()
	{
		if (controller.IsDash) return;

		if (!input.Move)
		{
			controller.weapon.back = true;
			if (controller.weapon.IsHide)
			{
				stateMachine.SwitchState(typeof(PlayerState_Idle));
			}
			else
			{
				controller.rigidbody.velocity = Vector2.zero;
				animator.Play("Break");
			}
		}
		if (controller.IsFall)
		{
			stateMachine.SwitchState(typeof(PlayerState_Fall));
		}
		if (input.Move)
		{
			stateMachine.SwitchState(typeof(PlayerState_Run));
		}
	}

	public override void PhysicUpdate()
	{
		controller.Dash(dashXPos);
	}
}
    
