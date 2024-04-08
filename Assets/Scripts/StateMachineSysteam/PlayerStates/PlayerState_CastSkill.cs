using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/CastSkill", fileName = "PlayerState_CastSkill")]

public class PlayerState_CastSkill : PlayerState
{
	public override void Enter()
	{
		Debug.Log("Now in CastSkill State");
	}
	public override void Exit()
	{

	}
	public override void LogicUpdate()
	{
		
		
	}

	public override void PhysicUpdate()
	{
		controller.Decelerate();
	}
}
