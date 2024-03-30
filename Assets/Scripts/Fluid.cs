using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Fluid : MonoBehaviour
{
	[SerializeField] private GameObject FluidColumn;
	[SerializeField] private Vector3 downOffset;
	[SerializeField] private Vector3 leftOffset;
	[SerializeField] private Vector3 rightOffset;

	[SerializeField] private LayerMask groundLayer;
	[SerializeField] private LayerMask fluidLayer;
	[SerializeField] private float flowVerticalSpeed;
	[SerializeField] private float flowhorizontalSpeed;

	//private List<GameObject> leftFluid;
	//private List<GameObject> righFluid;

	private void Awake()
	{
		
	}
	private void Update()
	{
		RayDetection();
	}

	private void RayDetection()
	{
		Debug.DrawRay(transform.position + downOffset, Vector2.down * 0.3f, Color.green);
		Debug.DrawRay(transform.position + leftOffset, Vector2.left * 0.3f, Color.red);
		Debug.DrawRay(transform.position + rightOffset, Vector2.right * 0.3f, Color.blue);

		RaycastHit2D downHit = Physics2D.Raycast(transform.position + downOffset, Vector2.down, 0.3f, groundLayer);
		RaycastHit2D leftHit = Physics2D.Raycast(transform.position + leftOffset, Vector2.left, 0.3f, groundLayer);
		RaycastHit2D rightHit = Physics2D.Raycast(transform.position + rightOffset, Vector2.right, 0.3f, groundLayer);

		if (!downHit)
		{
			//CreateFluidColumn();
			Debug.Log("downHit.collider == null");
		}
		if (!leftHit)
		{
			//CreateFluid(transform.position + Vector3.left);
			Debug.Log("leftHit.collider == null");
		}
		if (!rightHit) 
		{
			//CreateFluid(transform.position + Vector3.right);
			Debug.Log("rightHit.collider == null");
		}


	}

	private void CreateFluidColumn()
	{
		GameObject obj = Instantiate(FluidColumn);
		obj.transform.position = transform.position;
	}

	private void CreateFluid(Vector3 pos)
	{
		GameObject obj = Instantiate(this.gameObject);
		obj.transform.position = pos;
	}
}
