using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    private float movementInputDirection;
    public float movementSpeed = 10.0f;
    public float movementForceInAir;
    public float airDragMultiplier = 0.95f;
    public float variableJumpHeightMultiplier = 0.5f;
    public bool cantMove; 

    [Header("Jump")]
    private int amountOfJumpsLeft;
    private bool isJumping;
    public float jumpHoldTime;
    public Slider jumpSlider;
    public int amountOfJumps = 1;
    public float jumpForce = 16.0f;
    public LayerMask whatIsGround;
    public float groundCheckRadius;
    public float wallCheckDistance;
    Transform groundCheck;
    Transform wallCheck;
    public bool canShortJump;
    public bool canLongJump;

    [Header("Fly")]
    Coroutine staminaDecreaseCoroutine;
    Coroutine staminaIncreaseCoroutine;
    bool flyingCheck;
    public int maxFlyStamina;
    public Slider flySlider;
    public float flyForce;

    [Header("Checking conditions")]
    [SerializeField] private int facingDirection = 1;
    private bool isFacingRight = true;
    private bool isWalking;
    public bool isGrounded;
    private bool isTouchingWall;
    private bool canJump;
    [SerializeField] private bool canFly;
    public bool isFlying; 
    private bool flipped = false;

    [Header("Attached components")]
    private Rigidbody2D rb;
    public Animator anim;
    public Vector3 respawnPoint;
    public GameObject fallDetector;
    private CharacterStats characterStats;
    private Camera MainCamera;
    public GameObject shockwave;
    PlayerAttack PA;

    [Header("DASH")]
    public float dashSpeed;
    public float dashTime;
    [SerializeField] private bool _isDashing;
    public float distanceBetweenImages;
    public float dashCooldown;
    private float dashTimeLeft;
    private float lastImageXpos;
    private float lastDash = -100f;

    void Start()
    {
        PA = GetComponent<PlayerAttack>(); 
        characterStats = transform.GetComponentInParent<CharacterStats>(); 
        respawnPoint = transform.position;
        rb = GetComponent<Rigidbody2D>();
        groundCheck = transform.GetChild(0).transform;
        wallCheck = transform.GetChild(1).transform;
        amountOfJumpsLeft = amountOfJumps;
        staminaDecreaseCoroutine = StartCoroutine(DecreaseStats(1, 0));
        staminaIncreaseCoroutine = StartCoroutine(IncreaseStats(2, 0));
        jumpSlider.value = 0f;
        MainCamera = Camera.main;
    }

    void Update()
    {
        if (cantMove)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePosition; 
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();      
        CheckIfCanJump();
        CheckDash();
        var screenPos = MainCamera.WorldToScreenPoint(transform.position) + new Vector3(-80f, 0f, 0f);
        jumpSlider.transform.position = screenPos;
        flySlider.transform.position = screenPos;
        //Ask Professor
        if (Input.GetKeyDown(KeyCode.E))
        {
            shockwave.SetActive(true);
            shockwave.GetComponent<Shockwave>().CallShockwave(); 
        }
    }

    private void FixedUpdate()
    {
        CheckIfCanFly();
        ApplyMovement();
        CheckSurroundings();
    }

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
    }

    private void CheckIfCanJump()
    {
        if((isGrounded && rb.velocity.y <= 0))
        {
            amountOfJumpsLeft = amountOfJumps;
        }
        
        if (amountOfJumpsLeft <= 0)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }     
    }

    private void CheckMovementDirection()
    {
        if(isFacingRight && movementInputDirection < 0)
        {
            flipped = true;
            Flip();
        }
        else if(!isFacingRight && movementInputDirection > 0)
        {
            flipped = false;
            Flip();
        }

        if(rb.velocity.x != 0 && movementInputDirection != 0)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    private void UpdateAnimations()
    {
        anim.SetBool("Walking", isWalking);
        anim.SetBool("Grounded", isGrounded);
        anim.SetFloat("Speed", rb.velocity.y);
    }

    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");

        if (canShortJump)
        {
            if (isGrounded == true && Input.GetKeyDown(KeyCode.W))
            {
                rb.velocity = Vector2.up * jumpForce;
                //rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);  
            }
        }

        if (!PA.isCharlie)
        {
            if (canJump && canLongJump)
            {
                Jump();
            }
        }
        else
        {
            if (canFly)
            {
                Fly();
            }
        }

        if (Input.GetButtonDown("Dash"))
        {
            if (Time.time >= (lastDash + dashCooldown))
            {
                AttemptToDash();
            }  
        }

        fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);
    }

    private void CheckIfCanFly()
    { 
        if (!isFlying && flyingCheck && staminaDecreaseCoroutine != null)
        {
            flyingCheck = false;
            StopCoroutine(staminaDecreaseCoroutine);
            staminaIncreaseCoroutine = StartCoroutine(IncreaseStats(1, 1));
        }
        if (isFlying && !flyingCheck)
        {
            flyingCheck = true;
            staminaDecreaseCoroutine = StartCoroutine(DecreaseStats(1, 2));
            StopCoroutine(staminaIncreaseCoroutine);
        }

        if(flySlider.value <= 0f)
        {
            canFly = false; 
        }
        if(flySlider.value > 0)
        {
            canFly = true; 
        }

        if (isGrounded)
        {
            isFlying = false; 
        }
    }

    private void Fly()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            anim.SetBool("Flying", true);
            isFlying = true;
            flySlider.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
            //rb.AddForce(upForce);
            if (canFly)
            {
                Vector2 flyingVelocity = new Vector2(movementInputDirection * movementSpeed, flyForce);
                rb.velocity = flyingVelocity;
            }
        }

        if(Input.GetKeyUp(KeyCode.Space) && isFlying)
        {
            anim.SetBool("Flying", false);
            isWalking = false; 
            flySlider.gameObject.GetComponent<CanvasGroup>().alpha = 0f;
            isFlying = false;
        }
    }

    IEnumerator DecreaseStats(int interval, int amount)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            if (flySlider.value >= 0)
            {
                flySlider.value = Mathf.Max(flySlider.value - amount, 0);
            }
        }
    }

    IEnumerator IncreaseStats(int interval, int amount)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            if (flySlider.value <= maxFlyStamina - 1)
            {
                flySlider.value = Mathf.Max(flySlider.value + amount, 0);
            }
        }
    }

    private void Jump()
    {
        if (isGrounded == true && Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            jumpSlider.value = 0f;
            StartCoroutine("StartCounting");
            isJumping = true;
        }

        if (Input.GetKeyUp(KeyCode.Space) && isJumping == true)
        {
            StopCoroutine("StartCounting");
            if (jumpHoldTime < 0.3f)
            {
                anim.SetBool("Jumping", true);
                anim.SetBool("Grounded", true);
                rb.velocity = Vector2.up * jumpForce;
            }
            else
            {
                anim.SetBool("Jumping", true);
                anim.SetBool("Grounded", true);
                rb.velocity = Vector2.up * jumpForce * jumpHoldTime * 2.2f;
            }
            isJumping = false;
            jumpSlider.value = 0f;
            amountOfJumpsLeft--;
        }

        /*else if (isWallSliding && movementInputDirection == 0 && canJump) //Wall hop
        {
            isWallSliding = false;
            amountOfJumpsLeft--;
            Vector2 forceToAdd = new Vector2(wallHopForce * wallHopDirection.x * -facingDirection, wallHopForce * wallHopDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
        }
        else if((isWallSliding || isTouchingWall) && movementInputDirection != 0 && canJump)
        {
            isWallSliding = false;
            amountOfJumpsLeft--;
            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * movementInputDirection, wallJumpForce * wallJumpDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
        }*/
    }

    IEnumerator StartCounting()
    {
        for (jumpHoldTime = 0f; jumpHoldTime <= 0.75f; jumpHoldTime += Time.deltaTime)
        {
            jumpSlider.value = jumpHoldTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        jumpHoldTime = 0.75f;
        jumpSlider.value = jumpHoldTime;
        jumpSlider.maxValue = 0.75f;
    }

    private void ApplyMovement()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
        }
        else if(!isGrounded && movementInputDirection != 0)
        {
            Vector2 forceToAdd = new Vector2(movementForceInAir * movementInputDirection, 0);
            rb.AddForce(forceToAdd);

            if(Mathf.Abs(rb.velocity.x) > movementSpeed)
            {
                rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
            }
        }
        else if(!isGrounded && movementInputDirection == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }
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
            if (dashTimeLeft > 0)
            {
                if (flipped)
                {
                    transform.Translate(new Vector2(dashSpeed * facingDirection * -1, 0.0f));
                    //rb.velocity = new Vector2(dashSpeed * facingDirection, 0.0f);  
                    //The upper code can make a continuous after image effect to Benjamin. Can be used as speed powerup for a speedrun level. 
                    //rb.velocity = new Vector2(dashSpeed * facingDirection, transform.position.y);  
                    //This one can enable effect also with jump and flying. 
                    dashTimeLeft -= Time.deltaTime;
                }
                else
                {
                    transform.Translate(new Vector2(dashSpeed * facingDirection, 0.0f));
                    //rb.velocity = new Vector2(dashSpeed * facingDirection, 0.0f); 
                    //The upper code can make a continuous after image effect to Benjamin. Can be used as speed powerup for a speedrun level. 
                    dashTimeLeft -= Time.deltaTime;
                }

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
    }

    private void Flip()
    {
        facingDirection *= -1;
        isFacingRight = !isFacingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);      
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "FallDetector")
        {
            transform.position = respawnPoint;
        }
        else if (collision.tag == "Checkpoint")
        {
            respawnPoint = transform.position;
        }
    }
}
