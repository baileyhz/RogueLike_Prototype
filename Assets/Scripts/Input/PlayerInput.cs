using System.Collections;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    PlayerInputAction action;

	[SerializeField] private float preInputHoldTime = 0.5f;

	public bool preJumpInput {get; set;}
	public float axes => action.Gameplay.Move.ReadValue<float>();
	public bool Up => action.Gameplay.Up.IsPressed();
	public bool Down => action.Gameplay.Down.IsPressed();
	public bool Jump => action.Gameplay.Jump.WasPressedThisFrame();
	public bool StopJump => action.Gameplay.Jump.WasReleasedThisFrame();
	public bool Move => axes != 0;
	public bool Dash => action.Gameplay.Dash.WasPressedThisFrame();
	public bool LightAttack => action.Gameplay.LightAttack.WasPerformedThisFrame();
	public bool HeavyAttack => action.Gameplay.HeavyAttack.WasPerformedThisFrame();
	public bool PressThrow => action.Gameplay.Throw.WasPerformedThisFrame();
	public bool ReleasThrow => action.Gameplay.Throw.WasReleasedThisFrame();
	public bool HoldThrow => action.Gameplay.HoldThrow.WasPerformedThisFrame();

	private void Awake()
	{
		action = new PlayerInputAction();
	}

	public void EnablePlayerInput()
	{
		action.Gameplay.Enable();
	}

	private void OnEnable()
	{
		action.Gameplay.Jump.canceled += delegate { preJumpInput = false; };
	}

	public void SetJumpPreInput()
	{
		StopCoroutine(PreJumpInputCoroutine());
		StartCoroutine(PreJumpInputCoroutine());
	}

	IEnumerator PreJumpInputCoroutine()
	{
		preJumpInput = true;
		yield return new WaitForSeconds(preInputHoldTime);
		preJumpInput = false; 
	}

	
}
