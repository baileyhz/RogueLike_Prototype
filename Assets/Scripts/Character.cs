using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour,IAttackable
{
	[SerializeField] public List<Bless> blesses = new List<Bless>();

	[SerializeField] private float maxHp;
	[SerializeField] private float hp;
	[SerializeField] private float attackDamge;

	[SerializeField] private float maxTenacity;
	[SerializeField] private float tenacity;
	[SerializeField] private float deTenacity;
	[SerializeField] private float tenacityResetTime;
	private float hitRecoverTime;
	private float tenacityTimer;
	private float recoverTimer;
	
	public bool IsHitRecover;

	private Rigidbody2D rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		hp = maxHp;
		tenacity = maxTenacity;
		blesses.Add(new Bless_1());
	}

	private void Update()
	{
		foreach (Bless b in blesses)
		{
			b.UpDate(this);
		}

		HitRecover();
		TenacityResetJudge();
	}
	public void Injuried(Character character)
	{
		if (character.attackDamge >= hp)
		{
			hp = 0;
		}
		else
		{
			hp -= character.attackDamge;
		}
	}

	public void Knockback(Character character, Vector2 kbForce)
	{
		tenacityTimer = 0;

		if (!IsHitRecover) return;

		if (character.transform.localScale.x < 0)
		{
			rb.velocity += new Vector2(-kbForce.x, kbForce.y);
		}
		else
		{
			rb.velocity += kbForce;
		}
	}

	public void HitRecoverJudge(Character character,float deTenacityMultiply,float hr)
	{
		tenacityTimer = 0;

		tenacity -= character.deTenacity * deTenacityMultiply;

		if (tenacity<=0)
		{
			IsHitRecover = true;
			hitRecoverTime = hr;
			tenacity = 0;
		}
		else
		{
			IsHitRecover = false;
		}
	}
	private void TenacityResetJudge()
	{
		if (IsHitRecover) return;

		tenacityTimer += Time.deltaTime;

		if (tenacityTimer >= tenacityResetTime)
		{
			tenacityTimer = 0;
			tenacity = maxTenacity;
		}
	}
	private void HitRecover()
	{
		if (!IsHitRecover) return;

		hitRecoverTime -= Time.deltaTime;

		if (hitRecoverTime <= 0)
		{
			IsHitRecover = false;
			hitRecoverTime = 0;
		} 
	}
}

