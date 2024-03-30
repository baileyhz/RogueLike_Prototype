using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
	[SerializeField] public Vector2 attackDirection;
	[SerializeField] public Vector2 knockbackForce;
	[SerializeField] private GameObject player;
	[SerializeField] private float deTenacityMultiply;
	[SerializeField] private float hitRecoverTime;


	private void Awake()
	{
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.TryGetComponent(out IAttackable attackble) && collision.gameObject != player)
		{
			attackble.Injuried(player.GetComponent<Character>());
			attackble.HitRecoverJudge(player.GetComponent<Character>(),deTenacityMultiply,hitRecoverTime);
			attackble.Knockback(player.GetComponent<Character>(),knockbackForce);
		}
	}
}
