using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IThrowable
{
	void SetDirection(Vector2 dir);
	void ProjectileMotion();
	
}
