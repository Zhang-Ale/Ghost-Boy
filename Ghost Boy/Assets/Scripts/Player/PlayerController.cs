using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("MOVEMENT")]
    private float movementInputDirection;

    private int amountOfJumpsLeft;
    [SerializeField] private int facingDirection = 1;

    private bool isFacingRight = true;
    private bool isWalking;
    public bool isGrounded;
    private bool isTouchingWall;
    private bool canJump;
    private bool flipped = false;

    private Rigidbody2D rb;
    private Animator anim;

    private bool isJumping;
    public float holdTime;
    public Slider slider;
    public int amountOfJumps = 1;
    public Vector3 respawnPoint;
    public GameObject fallDetector;
    private CharacterStats characterStats; 
    public float movementSpeed = 10.0f;
    public float jumpForce = 16.0f;
    public float groundCheckRadius;
    public float wallCheckDistance;
    public float movementForceInAir;
    public float airDragMultiplier = 0.95f;
    public float variableJumpHeightMultiplier = 0.5f;
    private Camera MainCamera;
    public Transform groundCheck;
    public Transform wallCheck;

    public LayerMask whatIsGround;

    [Header("DASH")]
    public float dashSpeed;
    public float dashTime;
    [SerializeField] private bool _isDashing;
    public float distanceBetweenImages;
    public float dashCooldown;

    private float dashTimeLeft;
    private float lastImageXpos;
    private float lastDash = -100f;
    public SceneManagement SM; 
    public GreenBin GB;
    private bool canShortJump = false;
    public DialogueTwoImage D2Image;
    private bool canLongJump = false;

    [Header("DIALOGUES")]
    public GameObject Interactable1;
    public GameObject Dialogue1;
    public GameObject Interactable2;
    public GameObject Dialogue2;

    void Start()
    {
        characterStats = transform.GetComponentInParent<CharacterStats>(); 
        respawnPoint = transform.position;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJumps;
        slider.value = 0f;
        canShortJump = false;
        canLongJump = false;
        MainCamera = Camera.main;
    }

    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
        CheckDash();
        var screenPos = MainCamera.WorldToScreenPoint(transform.position) + new Vector3(-50f, 0f, 0f);
        slider.transform.position = screenPos;
    }

    private void FixedUpdate()
    {
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
        
        if (GB != null && GB._stopActivate)
        {
            canShortJump = true;
        }
        if (canShortJump)
        {
            if (isGrounded == true && Input.GetKeyUp(KeyCode.Space))
            {
                rb.velocity = Vector2.up * jumpForce;
                //rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
            }
        }

        if (D2Image!= null && D2Image._stopActivate)
        {
            canShortJump = false;
            canLongJump = true;
        }
        if (canLongJump)
        {
            Jump();
        }

        if (Input.GetButtonDown("Dash"))
        {
            if (Time.time >= (lastDash + dashCooldown))
            {
                AttemptToDash();
            }  
        }

        //fallDetector following the player at x
        fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);
    }

    private void Jump()
    {
        if (isGrounded == true && Input.GetKeyDown(KeyCode.Space) && canJump)
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
                anim.SetBool("Jumping", true);
                anim.SetBool("Grounded", true);
                rb.velocity = Vector2.up * jumpForce;
            }
            else
            {
                anim.SetBool("Jumping", true);
                anim.SetBool("Grounded", true);
                rb.velocity = Vector2.up * jumpForce * holdTime * 2.2f;
            }
            isJumping = false;
            slider.value = 0f;
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
        for (holdTime = 0f; holdTime <= 0.75f; holdTime += Time.deltaTime)
        {
            slider.value = holdTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        holdTime = 0.75f;
        slider.value = holdTime;
        slider.maxValue = 0.75f;
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
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

        if(collision.gameObject == Interactable1)
        {
            Dialogue1.SetActive(true);
            Dialogue2.SetActive(false);
        }

        if (collision.gameObject == Interactable2)
        {
            Dialogue2.SetActive(true);
            Dialogue1.SetActive(false);
        }

        if (collision.tag == "LoadPreviousLevel")
        {
            StartCoroutine(SM.Previous_Scene());
        }

        if (collision.tag == "LoadNextLevel")
        {

            StartCoroutine(SM.Next_Scene());
        }
    }
}
