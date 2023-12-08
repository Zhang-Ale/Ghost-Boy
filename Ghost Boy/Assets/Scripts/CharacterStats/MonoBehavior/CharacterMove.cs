using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

public class CharacterMove : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 600f;                          // Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching
	public float airDragMultiplier = 0.95f;
	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	[SerializeField] private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private int facingDirection = 1;
	private Vector3 m_Velocity = Vector3.zero;
	public Animator animator;
	public float runSpeed = 40f;
	float horizontalMove = 0f;
	bool crouch = false;
	private bool isJumping;
	public float holdTime;
	public Slider slider;
	public Vector3 respawnPoint;
	public GameObject fallDetector;
	[Header("DASH")]
	public float dashSpeed;
	public float dashTime;
	private bool _isDashing;
	public float distanceBetweenImages;
	public float dashCooldown;

	private float dashTimeLeft;
	private float lastImageXpos;
	private float lastDash = -100f;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;

	private void Awake()
	{
		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();
	}
	private void Start()
	{
		respawnPoint = transform.position;
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		slider.value = 0f;
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLanding();
			}
		}
		Move(horizontalMove * runSpeed * Time.fixedDeltaTime, crouch);
	}

	private void OnLanding()
	{
		animator.SetBool("Jumping", false);
		animator.SetBool("Grounded", false);
	}

	private void Update()
	{
		horizontalMove = Input.GetAxisRaw("Horizontal");
		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

		Jump();

		//Crouching();

		//fallDetector following the player at x
		fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);

		CheckMovementDirection();

		if (Input.GetButtonDown("Dash"))
		{
			if(Time.time >= (lastDash + dashCooldown))
			AttemptToDash();			
		}

		CheckDash();
	}

	private void Crouching()
	{
		/*if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }*/
	}

	private void AttemptToDash()
	{
		_isDashing = true;
		dashTimeLeft = dashTime;
		lastDash = Time.time;
		
		PlayerAfterImagePool.Instance.GetFromPool();
		lastImageXpos = transform.position.x;
	}

	private void CheckDash()
	{
		if (_isDashing)
		{
			m_Rigidbody2D.velocity = new Vector2(dashSpeed * facingDirection, m_Rigidbody2D.velocity.y);
			dashTimeLeft -= Time.deltaTime;

			if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
			{
				PlayerAfterImagePool.Instance.GetFromPool();
				lastImageXpos = transform.position.x;
			}
		}

		if (dashTimeLeft <= 0)
		{
			_isDashing = false;
		}
	}

	public void Jump()
	{
		if (m_Grounded == true && Input.GetKeyDown(KeyCode.Space))
		{
			slider.value = 0f;
			StartCoroutine("StartCounting");
			isJumping = true;
		}

		if (Input.GetKeyUp(KeyCode.Space) && isJumping == true)
		{
			StopCoroutine("StartCounting");
			if (holdTime < 0.3f)
			{
				animator.SetBool("Jumping", true);
				animator.SetBool("Grounded", true);
				m_Rigidbody2D.velocity = Vector2.up * m_JumpForce;
			}
			else
			{
				animator.SetBool("Jumping", true);
				animator.SetBool("Grounded", true);
				m_Rigidbody2D.velocity = Vector2.up * m_JumpForce * holdTime * 1.2f; 
			}
			isJumping = false;
			slider.value = 0f;
		}
	}

	IEnumerator StartCounting()
	{
		for (holdTime = 0f; holdTime <= 1f; holdTime += Time.deltaTime)
		{
			slider.value = holdTime;
			yield return new WaitForSeconds(Time.deltaTime);
		}
		holdTime = 1f;
		slider.value = holdTime;
		slider.maxValue = 1f;
	}

	public void Move(float move, bool crouch/*, bool jump, bool longJump*/)
	{
		
		// If crouching, check to see if the character can stand up
		if (!crouch)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				crouch = true;
			}
		}

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || !m_AirControl)
		{
			// If crouching
			if (crouch)
			{
				if (!m_wasCrouching)
				{
					m_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}

				// Reduce the speed by the crouchSpeed multiplier
				move *= m_CrouchSpeed;

				// Disable one of the colliders when crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = false;
			}
			else
			{
				// Enable the collider when not crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = true;

				if (m_wasCrouching)
				{
					m_wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
			}
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
		}

		if(!m_Grounded || m_AirControl)
		{
			Vector3 targetVelocity = new Vector2(move * airDragMultiplier, m_Rigidbody2D.velocity.y);
		}
	}

	private void CheckMovementDirection()
	{
		// If the input is moving the player right and the player is facing left...
		if (horizontalMove > 0 && !m_FacingRight)
		{
			// ... flip the player.
			Flip();
		}
		// Otherwise if the input is moving the player left and the player is facing right...
		else if (horizontalMove < 0 && m_FacingRight)
		{
			// ... flip the player.
			Flip();
		}
	}

	private void Flip()
	{
		facingDirection *= -1;

		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.tag == "FallDetector")
		{
			transform.position = respawnPoint;
		}else if(collision.tag == "Checkpoint")
		{
			respawnPoint = transform.position;
		}
	}
}
