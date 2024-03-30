using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bless_1 : Bless
{
	float CDTimer;
	float blessCD = 5f;
	bool canUseBless = true;

	public override string Name()
	{
		return "�Ĩ�^�_";
	}

	public override void UpDate(Character character)
	{
		if (canUseBless) return;

		CDTimer += Time.deltaTime;

		if (CDTimer>=blessCD)
		{
			Debug.Log("�Ĩ�^�_CD��s");
			canUseBless = true;
			CDTimer = 0;
		}
		else
		{
			canUseBless = false;
		}
	}

	public override void OnHit(PlayerController player,Character character)
	{
		if(player.IsHit && canUseBless && !player.canDash)
		{
			Debug.Log("�Ĩ�^�_");
			player.canDash = true;
			canUseBless = false;
		}
	}
}
