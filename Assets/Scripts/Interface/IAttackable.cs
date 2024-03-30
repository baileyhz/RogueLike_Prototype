using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
	void Injuried(Character character);
	void Knockback(Character character,Vector2 kbf);
	void HitRecoverJudge(Character character, float deTenacityMultiply,float hr);

}
