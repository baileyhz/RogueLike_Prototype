using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Bless : ScriptableObject
{
	public abstract string Name();

    public virtual void UpDate(Character character)
    {

    }
    public virtual void OnHit(PlayerController player, Character character)
    {

    }
    public virtual void CastSkill(Character character)
    {

    }
    public virtual void HitTarget(Character character)
    {

    }


}

