using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Throwable_Rock : MonoBehaviour,IThrowable
{
	[SerializeField] private float throwForce;
	[SerializeField] private float accTime;
	[SerializeField] private float deceleration;
	[SerializeField] private Vector2 direction;
	[SerializeField] private float timer = 0;
	[SerializeField] private LayerMask layerMask;
	[SerializeField] List<Vector3> offset = new List<Vector3>();
	private Rigidbody2D rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}
	void Start()
    {
		timer = 0;
	}

    void Update()
    {
		timer += Time.deltaTime;
    }
	private void FixedUpdate()
	{
		if (timer <= accTime)
		{
			rb.velocity = direction * throwForce;
		}
		else
		{
			float currentSpeed = rb.velocity.x;
			float dec = deceleration;

			currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, dec * Time.fixedDeltaTime);

			rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
		}
	}

	public void ProjectileMotion()
	{
		direction.Normalize();
	}

	public void SetDirection(Vector2 dir)
	{
		direction = dir;
		ProjectileMotion();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		DestructibleTiles dt;
		if(collision.gameObject.TryGetComponent<DestructibleTiles>(out dt))
		{
			dt.MakeDot(transform.position);
			foreach (Vector3 v in offset)
			{
				dt.MakeDot(transform.position + v);
			}
			Destroy(this.gameObject);
		}
	}

}
