using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
	[SerializeField] public BelialWeapon weapon;
	[SerializeField] public Transform handTrans;
	[SerializeField] private GameObject throwable;
	

	public GroundDetect groundDetect;

	private PlayerInput input;

    public Rigidbody2D rigidbody;

	#region MoveHorizontally
	[SerializeField,Header("玩家水平移動參數")]
	public float maxSpeed; //最高速度
    public float acceleration; //加速度 
    public float deceleration; //減速度
	public float turnSpeed; //轉向速度
	[SerializeField,Header("空中水平移動參數")]
	public float airMaxSpeed; //最高速度
	public float airAcceleration; //加速度
	public float airDeceleration; //減速度
	#endregion
	#region Jump
	[SerializeField,Header("跳躍相關")]
	public float jumpHeight; //跳躍高度
	public float gravity; //重力加速度
	public float jumpStartSpeed; //跳躍起始速度
	public float fallMaxSpeed; //下墜最高速度
	public int doubleJumpLimit;//二段跳次數
	public int doubleJumpCount = 0;
	#endregion
	#region Dash
	[SerializeField, Header("衝刺相關")]
	public float dashSpeed; //衝刺速度
	public float dashDistance; //衝刺長度
	public float dashCD; //衝刺冷卻
	[SerializeField] private float maxDashTime;
	
	private float maxDashTimer;
	private float dashTimer;
	#endregion
	#region Wall
	[SerializeField, Header("牆面相關")]
	public float wallJumpY; 
	public float wallJumpX;
	public float wallJumpSpeed; //牆跳速度
	public float walllJumpTime; //牆跳持續時間
	public float wallCancel; //取消攀牆
	public float wallDownSpeed; //下滑速度
	#endregion
	#region Throw
	[SerializeField, Header("投擲相關")]
	private float throwLength;
	[SerializeField] private GameObject throwLine;
	[SerializeField] private Vector3 offsetPos;
	[SerializeField] public float throwAngle;
	[SerializeField] private float angleSearchSpeed;
	[SerializeField] private LayerMask throwLayerMask;
	[SerializeField] private Vector3 throwDir;
	[SerializeField] private Vector3 targetDir;
	[SerializeField] private bool IsAimTarget;
	#endregion
	//public float playerSpeed => math.abs(rigidbody.velocity.x);
	public bool IsGrounded => groundDetect.IsGround;
	public bool IsTop => groundDetect.IsTop;
    public bool IsFall => rigidbody.velocity.y < 0f && !IsGrounded;
	public bool IsLeftWall => groundDetect.IsLeftWall;
	public bool IsRightWall => groundDetect.IsRightWall;
	
	[Header("條件判斷")]
	public bool IsJumping;
	public bool IsDash;
	public bool canDash;
	public bool canDoubleJump;
	public bool IsHit;
	public bool IsTurning;
	public bool canHit = true;
	public bool canCombo;

	private void Awake()
    {
        groundDetect = GetComponentInChildren<GroundDetect>();
        rigidbody = GetComponent<Rigidbody2D>();
		input = GetComponent<PlayerInput>();
		Physics2D.gravity = new Vector2(0, gravity);
    }
	private void Start()
	{
		input.EnablePlayerInput();
	}
	private void Update()
	{
		DashCD();
		DoubleJumpReset();
	}

	public void Move()
	{
		float horizontalInput = input.axes;
		float currentSpeed = rigidbody.velocity.x;
		float targetSpeed;
		float acc;
		float dec;

		if (IsGrounded)
		{
			targetSpeed = horizontalInput * maxSpeed;
			acc = acceleration;
			dec = deceleration;
		}
		else
		{
			targetSpeed = horizontalInput * airMaxSpeed;
			acc = airAcceleration;
			dec = airDeceleration;
		}
		
		//如果同時按下則不移動
		if (Keyboard.current.aKey.isPressed && Keyboard.current.dKey.isPressed) targetSpeed = 0f;

		//加減速度判斷
		if (Mathf.Abs(targetSpeed) > 0)
		{
			currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acc * Time.fixedDeltaTime);
		}
		else
		{
			currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, dec * Time.fixedDeltaTime);
		}
		//轉身判斷
		if ((rigidbody.velocity.x < 0 && horizontalInput > 0) || (rigidbody.velocity.x > 0 && horizontalInput < 0))
		{
			IsTurning = true;
			turnSpeed = maxSpeed * 3;
			currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, turnSpeed * Time.fixedDeltaTime);
			rigidbody.velocity = new Vector2(currentSpeed, rigidbody.velocity.y);
		}
		else
		{
			IsTurning = false;
		}

		rigidbody.velocity = new Vector2(currentSpeed, rigidbody.velocity.y);
		
		if (horizontalInput == 0) return;
		
		//判斷是否轉身
		if (horizontalInput > 0)
		{
			transform.localScale = new Vector3(1, 1, 1);
		}
		else
		{
			transform.localScale = new Vector3(-1, 1, 1);
		}
	}
	public void Jump()
	{
		//float targetY = startingY + jumpHeight;
		//float currentY = transform.position.y;

		float v_init = Mathf.Sqrt(2 * Physics.gravity.magnitude * rigidbody.gravityScale * jumpHeight);
		float force = rigidbody.mass * v_init;
		rigidbody.AddForce(Vector2.up * force, ForceMode2D.Impulse);

		//// 計算跳到最高處的進度
		//float jumpProgress = Mathf.Clamp01((currentY - startingY) / jumpHeight);

		//// 計算跳躍高度
		//float jumpVelocity = Mathf.Lerp(jumpStartSpeed, 0f, Mathf.Pow(jumpProgress, 2));

		//// 如果跳躍進度接近1或超過目標高度時，將跳躍速度設為0
		//if (jumpProgress >= 0.95f || currentY >= targetY)
		//{
		//	jumpVelocity = 0f;
		//}
		//rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpVelocity);
	}
	public void Dash(float startingX)
	{
		if (IsDash == false) return;
			
		canDash = false;
		canHit = false;
		
		maxDashTimer -= Time.deltaTime;
		float targetX;
		float currentX = transform.position.x;
		float dashVelocity;

		if (transform.localScale.x < 0) 
		{
			targetX = startingX - dashDistance; //左邊
			dashVelocity = dashSpeed * -1;
		}
		else
		{
			targetX = startingX + dashDistance; //右邊
			dashVelocity = dashSpeed;
		}

		if (dashVelocity > 0 && currentX >= targetX)
		{
			IsDash = false;
			if (currentX >= targetX * 0.8)
			{
				canHit = true;
			}
			canHit = true;
			return;
		}
		else if (dashVelocity < 0 && currentX <= targetX)
		{
			IsDash = false;
			if (currentX <= targetX*0.8)
			{
				canHit = true;
			}
			canHit = true;
			return;
		}
		if (maxDashTimer <= 0 && (IsLeftWall||IsRightWall))
		{
			maxDashTimer = maxDashTime;

			IsDash = false;
			canHit = true;
		}

		rigidbody.velocity = new Vector2(dashVelocity,0f);
	}
	public void Climb()
	{
		if (IsRightWall)
		{
			transform.localScale = new Vector3(-1, 1, 1);
		}
		else if (IsLeftWall)
		{
			transform.localScale = new Vector3(1, 1, 1);
		}
		rigidbody.velocity = new Vector2(0,wallDownSpeed);
	}
	public void WallJump()
	{
		Vector2 angle = new Vector2(wallJumpX,wallJumpY);
		angle = angle.normalized;

		rigidbody.velocity = new Vector2 (angle.x * wallJumpSpeed * transform.localScale.x, angle.y * wallJumpSpeed);
	}
	public void Fall()
	{
		if (rigidbody.velocity.y < fallMaxSpeed) rigidbody.velocity = new Vector3(rigidbody.velocity.x, fallMaxSpeed);
	}
	private void DashCD()
	{
		if (!IsDash && !canDash)
		{
			dashTimer -= Time.deltaTime;

			if (dashTimer <= 0)
			{
				canDash = true;
				dashTimer = dashCD;
			}
		}
		else if(canDash)
		{
			dashTimer = dashCD;
			return;
		}
	}
	private void DoubleJumpReset()
	{
		if (doubleJumpCount >= doubleJumpLimit)
		{
			canDoubleJump = false;
		}
		else
		{
			canDoubleJump = true;
		}
	}
	public void AttackMoveX(AnimationEvent animationEvent)
	{
		float posX = animationEvent.floatParameter;
		rigidbody.velocity = new Vector2(posX * transform.localScale.x, rigidbody.velocity.y);
	}
	public void AttackMoveY(AnimationEvent animationEvent)
	{
		float posY = animationEvent.floatParameter;
		rigidbody.velocity = new Vector2(rigidbody.velocity.x, posY);
	}
	public void Decelerate()
	{
		float currentSpeed = rigidbody.velocity.x;
		float dec = deceleration;

		currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, dec * Time.fixedDeltaTime);

		rigidbody.velocity = new Vector2(currentSpeed, rigidbody.velocity.y);
	}
	public void ThrowThrowable()
	{
		GameObject obj = Instantiate(throwable);
		obj.transform.position = handTrans.position;
		
		if (IsAimTarget)
		{
			obj.GetComponent<IThrowable>().SetDirection(new Vector2(throwDir.x, throwDir.y));
			IsAimTarget = false;
			return;
		}

		if (input.Down && !input.Move)
		{
			obj.transform.position = new Vector3(transform.position.x,transform.position.y + 1.3f, 0) ;
			obj.GetComponent<IThrowable>().SetDirection(new Vector2(0, -1));
		}
		else if (input.Up && !input.Move)
		{
			obj.GetComponent<IThrowable>().SetDirection(new Vector2(0, 1));
		}
		else if (input.Up && input.Move)
		{
			obj.GetComponent<IThrowable>().SetDirection(new Vector2(input.axes, 1));
		}
		else if (input.Down && input.Move)
		{
			obj.GetComponent<IThrowable>().SetDirection(new Vector2(input.axes, -1));
		}
		else
		{
			obj.GetComponent<IThrowable>().SetDirection(new Vector2(transform.localScale.x,0));
		}
	}
	public void SetThrowLine(bool enbled)
	{
		if (!enbled)
		{
			throwLine.SetActive(enbled);
			return;
		}

		throwLine.SetActive(enbled);
		
		if (throwAngle>=90)
		{
			throwAngle = 90f;
		}
		else if (throwAngle<=-90)
		{
			throwAngle = -90f;
		}
		throwDir = new Vector3(Mathf.Cos(throwAngle * Mathf.Deg2Rad), Mathf.Sin(throwAngle * Mathf.Deg2Rad), 0);
		throwDir.Normalize();
		throwDir *= transform.localScale.x;
		Debug.DrawRay(transform.position + offsetPos, throwDir * throwLength);
		RaycastHit2D hit = Physics2D.Raycast(transform.position + offsetPos, throwDir, throwLength, throwLayerMask);
		if (hit.collider != null)
		{
			IsAimTarget = true;
			angleSearchSpeed = 50f;
			Vector3 tLDir = hit.transform.position - (transform.position + offsetPos);
			tLDir.Normalize();
			tLDir *= transform.localScale.x;
			float tLRad = Mathf.Atan2(tLDir.y, tLDir.x);
			float tLDeg = tLRad * Mathf.Rad2Deg;
			throwLine.transform.rotation = Quaternion.Euler(0,0,tLDeg);
			targetDir = tLDir;
			
			if (!input.Up && !input.Down)
			{
				throwAngle = tLDeg;
			}
		}
		else
		{
			IsAimTarget = true;
			angleSearchSpeed = 100f;
			throwLine.transform.rotation = Quaternion.Euler(0, 0, throwAngle);
			targetDir = throwDir;
		}

		if ((transform.localScale.x < 0 && input.axes > 0) || (transform.localScale.x > 0 && input.axes < 0))
		{
			throwAngle = 0;
			throwLine.transform.rotation = Quaternion.Euler(0, 0, 0);
		}

		if (input.Down)
		{
			throwAngle -= angleSearchSpeed * Time.deltaTime * transform.localScale.x;
		}
		else if (input.Up)
		{
			throwAngle += angleSearchSpeed * Time.deltaTime * transform.localScale.x;
		}
	}
	public void Turn()
	{
		if (input.axes > 0)
		{
			transform.localScale = new Vector3(1,1,1); 
		}
		else if (input.axes < 0)
		{
			transform.localScale = new Vector3(-1, 1, 1);
		}
	}
	
}