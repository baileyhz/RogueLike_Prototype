using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR;
using static UnityEngine.GraphicsBuffer;

public class BelialWeapon : MonoBehaviour
{
	public Transform belialHandTrans;
	public Transform belialTrans;

	[SerializeField] float maxSpeed = 20f;
	[SerializeField] float accelerationDistance = 10.0f;
	[SerializeField] float smoothSpeed = 5.0f;
	[SerializeField] float safeDistance;
	[SerializeField] float weaponBackSpeed;
	[SerializeField] private float backRestTime = 0.3f;

	[SerializeField] Vector3 handPos;
    [SerializeField] Vector2 handbox;
    [SerializeField] LayerMask handLayer;

	public bool IsHold => Physics2D.OverlapBoxNonAlloc(transform.position + handPos, handbox, 0f, collider2Ds, handLayer) != 0;
	
	public bool back = false;
	public bool IsHide;

	private float backTimer;

	private Rigidbody2D rb;
	private SpriteRenderer spriteRenderer;
	private Collider2D collider2D;
	private Animator animator;

	Collider2D[] collider2Ds = new Collider2D[1];


	private void Awake()
	{
		collider2D = GetComponent<Collider2D>();
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		if (back)
		{
			backTimer += Time.fixedDeltaTime;
			if (backTimer >= backRestTime)
			{
				WeaponBack();
			}
			else
			{
				WeaponFollow();
			}
		}
		else
		{
			backTimer = 0;
			WeaponFollow();
		}
	}

	void Start()
    {
		back = false;
	}


	public void WeaponBack()
    {
		if (IsHold)
		{
			WeaponHide();
		}
		Vector3 targetPosition = new Vector3(belialHandTrans.position.x, belialHandTrans.position.y, 1);
		transform.position = Vector3.MoveTowards(transform.position, targetPosition, weaponBackSpeed * Time.fixedDeltaTime);
	}

    public void WeaponFollow()
	{
		animator.Play("BelialWeapon_Flot");
		float scaleMultiplier = Mathf.Sign(-belialTrans.localScale.x);

		Vector3 targetPosition = new Vector3(belialHandTrans.position.x + 
			(safeDistance * scaleMultiplier),belialHandTrans.position.y,1);

		float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

		if (distanceToTarget < 0.1f)
		{
			rb.velocity = Vector2.zero;
		}
		else
		{
			float currentSpeed = (distanceToTarget > accelerationDistance) ? maxSpeed : smoothSpeed;
			
			rb.velocity = (targetPosition - transform.position).normalized * currentSpeed;

			if (distanceToTarget < 0.5f)
			{
				float deceleration = currentSpeed * Time.fixedTime;
				rb.velocity = Vector2.MoveTowards(rb.velocity, Vector2.zero, deceleration);
			}
		}
	}
	public void WeaponReset()
	{
		if (IsHide == true)
		{
			spriteRenderer.enabled = true;
			collider2D.enabled = true;
			back = false;
			transform.position = new Vector3(belialHandTrans.position.x, belialHandTrans.position.y, 1);
			transform.localScale = belialTrans.localScale;
			animator.Play("BelialWeapon_Idle");
			IsHide = false;
		}
		
	}
	public void WeaponHide()
	{
		if (IsHide == false)
		{
			collider2D.enabled = false;
			spriteRenderer.enabled = false;
			IsHide = true;
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(transform.position + handPos, handbox);
	}
}
