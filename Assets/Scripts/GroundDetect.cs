using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetect : MonoBehaviour
{
	[SerializeField, Header("浪代")]
	Vector2 downDetectionBox = new Vector2(0.1f,0.1f);
	[SerializeField]
	Vector3 downDetectionPos = new Vector3(0f,0f,0f);
	
	[SerializeField, Header("繷郴浪代")]
	Vector2 topDetectionBox = new Vector2(0.1f, 0.1f);
	[SerializeField]
	Vector3 topDetectionPos = new Vector3(0f, 0f, 0f);

	[Header("鲤浪代")]
	[SerializeField, Header("オ浪代")]
	Vector2 leftDetectionBox = new Vector2(0.1f, 0.1f);
	[SerializeField]
	Vector3 leftDetectionPos = new Vector3(0f, 0f, 0f);
	[SerializeField, Header("浪代")]
	Vector2 rightDetectionBox = new Vector2(0.1f, 0.1f);
	[SerializeField]
	Vector3 rightDetectionPos = new Vector3(0f, 0f, 0f);
	[SerializeField]
	float wallDetectionResetTime;
	
	bool wallResetTrigger = true;
	

	[SerializeField] 
	LayerMask groundLayer;

	Collider2D[] collider2Ds = new Collider2D[1];
    public bool IsGround
	{
		get
		{
			return Physics2D.OverlapBoxNonAlloc(transform.position + downDetectionPos, downDetectionBox, 0f,collider2Ds,groundLayer) != 0;
		}
	}

	public bool IsTop
	{
		get
		{
			return Physics2D.OverlapBoxNonAlloc(transform.position + topDetectionPos, topDetectionBox, 0f, collider2Ds, groundLayer) != 0;
		}
	}

	public bool IsLeftWall 
	{
		get
		{
			return Physics2D.OverlapBoxNonAlloc(transform.position + leftDetectionPos, leftDetectionBox, 0f, collider2Ds, groundLayer) != 0 && wallResetTrigger;
		}
	}
	public bool IsRightWall
	{
		get
		{
			return Physics2D.OverlapBoxNonAlloc(transform.position + rightDetectionPos, rightDetectionBox, 0f, collider2Ds, groundLayer) != 0 && wallResetTrigger;
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(transform.position + downDetectionPos, downDetectionBox);

		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(transform.position + topDetectionPos, topDetectionBox);

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(transform.position + leftDetectionPos, leftDetectionBox);
		Gizmos.DrawWireCube(transform.position + rightDetectionPos, rightDetectionBox);
	}

	public void ResetOnWall()
	{
		StopCoroutine(OnWallCoroutine());
		StartCoroutine(OnWallCoroutine());
	}
	IEnumerator OnWallCoroutine()
	{
		wallResetTrigger = false;
		yield return new WaitForSeconds(wallDetectionResetTime);
		wallResetTrigger = true;
	}
}
