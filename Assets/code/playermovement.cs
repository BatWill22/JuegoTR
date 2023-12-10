using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    //Variable Declaration

    [SerializeField] private Animator animator;
    
    public bool canMove = false;

    public float moveSpeed = 5f;
    public float jumpForce = 3000f;
    public float wallSlideSpeed = 2f;
    public float wallJumpCooldown = 0.2f; // Adjust this cooldown duration as needed.

    public LayerMask groundLayer;
    public LayerMask wallLayer;

    public static bool canWallJumpAndSlide = false; // New variable for enabling/disabling wall jump and slide
    public static bool canDoubleJump = false;
    public static bool canDash = false; // Can dash

    public static bool hasRedKey = false; // New variable for enabling/disabling wall jump and slide
    public static bool hasGreenKey = false;
    public static bool hasBlueKey = false; // Can dash

    public static int coinCount = 0;

    private float startTime;
    public static float elapsedTime;
    public static float bestTime = float.MaxValue;


    private Rigidbody2D rb;
    private Collider2D playerCollider;

    public Vector3 lastCheckGroundPosition; // Declare lastCheckGroundPosition here
    private Vector2 knockBackVelocity;

    public bool isGrounded = true;
    private bool isCheckGrounded = false;
    private bool isTouchingWall = false;
    private bool isWallSliding = false;
    private bool isWallOnRight = false;
    private bool isWallOnLeft = false;
    private bool jumpedFromRight = false;
    private bool wallJumpCooldownActive = false;
    private float timeSinceWallJump;
    private float wallJumpBoost = 1.5f;
    private bool keepJumping = false;
    private float jumpTimer = 0f;
    private float maxJumpDuration = 0.2f; // Maximum jump duration in seconds
    private bool remainDoubleJump = true;
    private bool hasBeenHit = false;

    private bool activateMovement = true; //variable to activate movement on x after knockback
    private bool moveJustX = false; //for when a player gets knocked back in y axis, to change just x velocity
    
    public int facingX = 1; // -1 for left, 1 for right
    public int facingY = 0; // -1 for down, 1 for up, 0 for no direction

    public float dashForce = 10000f; // Dash force
    public float dashDuration = 0.2f; // Dash duration
    public float dashCooldown = 2f; // Dash cooldown
    private bool isDashing = false; // Is currently dashing
    private float dashCooldownTimer = 0f; // Dash cooldown timer
    private float dashDurationTimer = 0f; // Dash duration timer
    private float timeSinceKnockBack = 0f;
    private float knockBackCooldown = 0.3f;

    public float delayBeforeMove = 1.0f;  // Adjust the delay time as needed
    private float moveTimer = 0f;

    private float pushDirectionPlayerX; 
    private float pushDirectionPlayerY; 
    private float pushForcePlayer;

    public bool gameOver = false;
    private bool starts;
    private bool notPaused = true;

    
    private bool rightRaycastHit;
    private bool leftRaycastHit;

    [Header("Sounds")]
    [SerializeField] private AudioSource normalWalking;

    public void ResetToLastCheckZonePosition()
    {
        hasBeenHit = true;
        isDashing = false;
        rb.velocity = Vector2.zero; // Stop the player's movement
        rb.gravityScale = 0.0f;
        moveTimer = 0f;
    }
    
    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.layer == groundLayer) // Assuming the ground has a specific layer
    //     {
    //         lastCheckGroundPosition = transform.position;
    //     }
    // }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        lastCheckGroundPosition = transform.position; // Initialize it with the player's initial position
        remainDoubleJump = canDoubleJump;
        canMove = false;
        starts = true;
        startTime = Time.time;
        canWallJumpAndSlide = false; // New variable for enabling/disabling wall jump and slide
        canDoubleJump = false;
        canDash = false; // Can dash

        hasRedKey = false; // New variable for enabling/disabling wall jump and slide
        hasGreenKey = false;
        hasBlueKey = false; // Can dash

        coinCount = 0;
    }

    private void UpdateAnimations()
    {
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("Y.rb.velocity", rb.velocity.y);
        animator.SetBool("Wallslide", isWallSliding);
        // Debug.Log("velocity en x: " + rb.velocity.x);
        if(isGrounded && (rb.velocity.x != 0f) && ( Input.GetKeyDown("a") || Input.GetKey("a") || Input.GetKeyDown("d") || Input.GetKey("d")))
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        if (facingY == 1)
        {
            animator.SetFloat("Yfacing", 1.0f);
        }
        else if (facingY == -1)
        {
            animator.SetFloat("Yfacing", -1.0f);
        }
        else 
        {
            animator.SetFloat("Yfacing", 0.0f);
        }
        if (isDashing)
        {
            animator.SetTrigger ("Dash");
        }
    }

    private void Update()
    {
        UpdateAnimations();
        if (moveTimer <= delayBeforeMove)
        {
            moveTimer += Time.deltaTime;
            canMove = false;
        }

        if (moveTimer >= delayBeforeMove)
        {
            if (hasBeenHit && notPaused)
            {
                rb.gravityScale = 10f;
                transform.position = lastCheckGroundPosition; // Set the player's position to the last ground position
                hasBeenHit = false;
                canMove = true;
                animator.SetTrigger("Respawn");
            }
            if (gameOver)
            {
                SceneManager.LoadScene("EndGameMenu");
                gameOver = false;
                canMove = true;
            }
            if(starts)
            {
                starts = false;
                canMove = true;
            }
        }

        if (canMove)
        {
            rb.gravityScale = 10f;
            isCheckGrounded = playerCollider.IsTouchingLayers(groundLayer);
            float rayLengthFactor = 1.25f;
            //Calcular si isGrounded
            // Calculate positions for raycasts
            Vector3 rightRaycastOrigin = playerCollider.bounds.center + new Vector3(playerCollider.bounds.extents.x*0.85f, 0f, 0f);
            Vector3 leftRaycastOrigin = playerCollider.bounds.center - new Vector3(playerCollider.bounds.extents.x*0.85f, 0f, 0f);
            Vector3 centerRaycastOrigin = playerCollider.bounds.center;
            // Perform raycasts
            if (!isDashing)
            {
                rightRaycastHit = Physics2D.Raycast(rightRaycastOrigin, Vector2.down, playerCollider.bounds.extents.y*rayLengthFactor, wallLayer);
                leftRaycastHit = Physics2D.Raycast(leftRaycastOrigin, Vector2.down, playerCollider.bounds.extents.y*rayLengthFactor, wallLayer);
            }
            else
            {
                rightRaycastHit = false;
                leftRaycastHit = false;
            }
            bool centerRaycastHit = Physics2D.Raycast(centerRaycastOrigin, Vector2.down, playerCollider.bounds.extents.y*rayLengthFactor, wallLayer);
            // Check if any of the raycasts hit the ground
            isGrounded = /*playerCollider.IsTouchingLayers(groundLayer) || (playerCollider.IsTouchingLayers(wallLayer) &&*/ (rightRaycastHit || leftRaycastHit || centerRaycastHit /*|| isCheckGrounded*/);

            // debug break when is not grounded
            // if (!isGrounded)
            // {
            //     Debug.Log("is not grounded");
            //     //debug all values that give value to isGrounded
            //     Debug.Log("playerCollider.IsTouchingLayers(groundLayer): " + playerCollider.IsTouchingLayers(groundLayer));
            //     Debug.Log("playerCollider.IsTouchingLayers(wallLayer): " + playerCollider.IsTouchingLayers(wallLayer));
            //     Debug.Log("rightRaycastHit: " + rightRaycastHit);
            //     Debug.Log("leftRaycastHit: " + leftRaycastHit);
            //     Debug.Log("centerRaycastHit: " + centerRaycastHit);
            //     //Debug.Log("isCheckGrounded: " + isCheckGrounded);
                
            //     Debug.Break();
            // }
                    
            //show lines for raycasts
            Debug.DrawRay(rightRaycastOrigin, Vector2.down * (playerCollider.bounds.extents.y*rayLengthFactor), (playerCollider.IsTouchingLayers(groundLayer) || rightRaycastHit) ? Color.green : Color.red);
            Debug.DrawRay(leftRaycastOrigin, Vector2.down * (playerCollider.bounds.extents.y*rayLengthFactor), (playerCollider.IsTouchingLayers(groundLayer) || leftRaycastHit) ? Color.green : Color.red);
            Debug.DrawRay(centerRaycastOrigin, Vector2.down * (playerCollider.bounds.extents.y*rayLengthFactor), (playerCollider.IsTouchingLayers(groundLayer) || centerRaycastHit) ? Color.green : Color.red);

            // Calculate positions for raycasts on Y
            Vector3 upRaycastOrigin = playerCollider.bounds.center + new Vector3(0f, playerCollider.bounds.extents.y*0.85f, 0f);
            Vector3 downRaycastOrigin = playerCollider.bounds.center - new Vector3(0f, playerCollider.bounds.extents.y*0.85f, 0f);
            Vector3 centerRaycastOriginY = playerCollider.bounds.center;
            // Perform raycasts
            bool upRaycastHitRight = Physics2D.Raycast(upRaycastOrigin, Vector2.right, playerCollider.bounds.extents.x*rayLengthFactor, wallLayer);
            bool downRaycastHitRight = Physics2D.Raycast(downRaycastOrigin, Vector2.right, playerCollider.bounds.extents.x*rayLengthFactor, wallLayer);
            bool centerRaycastHitYRight = Physics2D.Raycast(centerRaycastOriginY, Vector2.right, playerCollider.bounds.extents.x*rayLengthFactor, wallLayer);

            Debug.DrawRay(upRaycastOrigin, Vector2.right * (playerCollider.bounds.extents.x*rayLengthFactor), upRaycastHitRight ? Color.green : Color.red);
            Debug.DrawRay(downRaycastOrigin, Vector2.right * (playerCollider.bounds.extents.x*rayLengthFactor), downRaycastHitRight ? Color.green : Color.red);
            Debug.DrawRay(centerRaycastOriginY, Vector2.right * (playerCollider.bounds.extents.x*rayLengthFactor), centerRaycastHitYRight ? Color.green : Color.red);
            
            bool upRaycastHitLeft = Physics2D.Raycast(upRaycastOrigin, Vector2.left, playerCollider.bounds.extents.x*rayLengthFactor, wallLayer);
            bool downRaycastHitLeft = Physics2D.Raycast(downRaycastOrigin, Vector2.left, playerCollider.bounds.extents.x*rayLengthFactor, wallLayer);
            bool centerRaycastHitYLeft = Physics2D.Raycast(centerRaycastOriginY, Vector2.left, playerCollider.bounds.extents.x*rayLengthFactor, wallLayer);

            Debug.DrawRay(upRaycastOrigin, Vector2.left * (playerCollider.bounds.extents.x*rayLengthFactor), upRaycastHitLeft ? Color.green : Color.red);
            Debug.DrawRay(downRaycastOrigin, Vector2.left * (playerCollider.bounds.extents.x*rayLengthFactor), downRaycastHitLeft ? Color.green : Color.red);
            Debug.DrawRay(centerRaycastOriginY, Vector2.left * (playerCollider.bounds.extents.x*rayLengthFactor), centerRaycastHitYLeft ? Color.green : Color.red);
            // calculate if it's touching a wall and not a roof
            isWallOnRight = playerCollider.IsTouchingLayers(wallLayer) && (upRaycastHitRight || centerRaycastHitYRight || (!isGrounded && downRaycastHitRight));
            isWallOnLeft = playerCollider.IsTouchingLayers(wallLayer) && (upRaycastHitLeft || centerRaycastHitYLeft || (!isGrounded && downRaycastHitLeft));

            // isWallOnRight = playerCollider.IsTouchingLayers(wallLayer) && Physics2D.Raycast(playerCollider.bounds.center, Vector2.right, playerCollider.bounds.extents.x + 0.05f, wallLayer);
            // isWallOnLeft = playerCollider.IsTouchingLayers(wallLayer) && Physics2D.Raycast(playerCollider.bounds.center, Vector2.left, playerCollider.bounds.extents.x + 0.05f, wallLayer);
            isTouchingWall = isWallOnLeft || isWallOnRight;
            if (Physics2D.Raycast(centerRaycastOrigin, Vector2.up, playerCollider.bounds.extents.x*rayLengthFactor, wallLayer) && !Physics2D.Raycast(playerCollider.bounds.center, Vector2.right, playerCollider.bounds.extents.x*rayLengthFactor, wallLayer) && !Physics2D.Raycast(playerCollider.bounds.center, Vector2.left, playerCollider.bounds.extents.x*rayLengthFactor, wallLayer ))
            {
                isTouchingWall = false;
                // Debug.Log("Pasa Por aki");
            }

            //Keep Checkground Position
            if (isCheckGrounded)
            {
                lastCheckGroundPosition = transform.position;
            }

            //determine which side of the wall
            // isWallOnRight = isTouchingWall && Physics2D.Raycast(playerCollider.bounds.center, Vector2.right, 1f, wallLayer);
            // isWallOnLeft = isTouchingWall && Physics2D.Raycast(playerCollider.bounds.center, Vector2.left, 1f, wallLayer);

            // Activate dash
            if (canDash && Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && dashCooldownTimer <= 0)
            {
                isDashing = true;
                dashDurationTimer = dashDuration;
                dashCooldownTimer = dashCooldown;
            }


            Vector2 velocity2 = rb.velocity;

            if (Input.GetKeyDown(KeyCode.Space)) //When press space down
            {
                animator.SetTrigger("Jump");
                if (isGrounded)
                {
                    velocity2 = new Vector2(rb.velocity.x, jumpForce);

                    keepJumping = true;
                    jumpTimer = 0f;

                    // animator.SetTrigger("Jump");
                }
                else if (isWallSliding && !wallJumpCooldownActive)
                {
                    Vector2 wallJumpDirection = isWallOnRight ? Vector2.left : Vector2.right;
                    isWallSliding = false;
                    jumpedFromRight = isWallOnRight;
                    velocity2 = wallJumpDirection * jumpForce;
                    wallJumpCooldownActive = true;
                    timeSinceWallJump = 0f;
                    keepJumping = true;
                    jumpTimer = 0f;
                    // animator.SetTrigger("Jump");
                }
                else if (canDoubleJump && remainDoubleJump)
                {
                    // animator.SetTrigger("Jump");
                    if (wallJumpCooldownActive)
                    {
                        wallJumpCooldownActive = false;
                    }
                    remainDoubleJump = false;
                    // Debug.Log("YA NO PUEDES PASARRR(SALTAR)");
                    velocity2 = new Vector2(rb.velocity.x, jumpForce);
                    keepJumping = true;
                    jumpTimer = 0f;
                }
                rb.velocity = velocity2;
            }

            if (canWallJumpAndSlide)
            {
                if (Input.GetKey("d") || Input.GetKey("a") && !isGrounded)
                {
                    isWallSliding = isTouchingWall;
                }

                if (isGrounded || (isWallSliding && isWallOnRight && Input.GetKey("a")) || (isWallSliding && isWallOnLeft && Input.GetKey("d")) || !isTouchingWall)
                {
                    isWallSliding = false;
                }
            }

            if (Input.GetKeyUp(KeyCode.Space)) // Stop jumping when stop pressing space
            {
                keepJumping = false;
            }
            if ((isGrounded || isWallSliding) && canDoubleJump) //allow double jump when touching ground/wall
            {
                remainDoubleJump = true;
                // Debug.Log("RemainDoubleJump");
                // if (isWallSliding)
                // {
                //     Debug.Log("Wall Sliding: " + isWallSliding);
                // }
                // if (isGrounded)
                // {
                //     Debug.Log("Grounded: " + isGrounded);
                // }
                // Debug.Log("Is Grounded" + isGrounded);
            }

            // Log variables to the console.
            //Debug.Log("y Velocity: " + rb.velocity.y);
            //Debug.Log("Touching Wall: " + isTouchingWall);
            //Debug.Log("Wall Sliding: " + isWallSliding);
            //Debug.Log("wallJump Cooldown: " + wallJumpCooldownActive);
            //Debug.Log("Keep Jumping: " + keepJumping);
            //Debug.Log("left Wall: " + isWallOnLeft);
            //Debug.Log("Remain Double Jump: " + remainDoubleJump);
            // Debug.Log("x: " + facingX);
            // Debug.Log("y: " + facingY);
            //Debug.Log("is dashing: " + isDashing);
            //Debug.Log("jumped from right: " + jumpedFromRight);
            //Debug.Log("grounded: " + isGrounded);

            float verticalInput = Input.GetAxisRaw("Vertical"); // Added vertical input

            //Facing direction in X
            if ( isWallSliding)
            {
                //Debug.Log("x: " + facingX);
                if (isWallOnRight)
                {
                    facingX = -1;
                }
                else
                {
                    facingX = 1;
                }
            }
            else if (wallJumpCooldownActive)
            {
                if (jumpedFromRight)
                {
                    facingX = -1;
                }
                else 
                {
                    facingX = 1;
                }
            }
            else if (!isDashing)
            {
                if (Input.GetKeyDown("d") || Input.GetKey("d"))
                {
                    facingX = 1;
                }
                else if (Input.GetKeyDown("a") || Input.GetKey("a"))
                {
                    facingX = -1;
                }
            }

            // Update facingY based on vertical input
            if (verticalInput > 0)
            {
                facingY = 1;
            }
            else if (verticalInput < 0)
            {
                facingY = -1;
            }
            else
            {
                facingY = 0;
            }


            // para activar y desactivar las habilidades
            // if (Input.GetKeyDown("i"))
            // {
            //     canDash = !canDash;
            // }
            // if (Input.GetKeyDown("o"))
            // {
            //     canDoubleJump = !canDoubleJump;
            // }
            // if (Input.GetKeyDown("p"))
            // {
            //     canWallJumpAndSlide = !canWallJumpAndSlide;
            // }
        }
        else
        {
            // Debug.Log("rb velocity es 0");
            rb.gravityScale = 0f;
            rb.velocity = Vector2.zero;
        }

        if (facingX == 1)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (facingX == -1)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }

    void FixedUpdate()
    {
        // animator.SetBool("isRunning", false);
        if (canMove)
        {
            // Update dash timers
            if (dashCooldownTimer > 0)
            {
                dashCooldownTimer -= Time.deltaTime;
            }
            if (dashDurationTimer > 0)
            {
                dashDurationTimer -= Time.deltaTime;
            }
            if (dashDurationTimer <= 0)
            {
                isDashing = false;
            }

            Vector2 velocity = rb.velocity;

            float horizontalInput = Input.GetAxisRaw("Horizontal");

            if (!activateMovement)
            {
                // Debug.Log(pushDirectionPlayer);
                // Debug.Log("velocitat:"+rb.velocity.x); 
                velocity = knockBackVelocity;
            }
            else if (isTouchingWall && !isGrounded && !keepJumping && isWallSliding && !wallJumpCooldownActive && !isDashing) //movement in wallsliding
            {
                velocity = new Vector2(0f, -wallSlideSpeed);
            }
            else if (keepJumping && Input.GetKey(KeyCode.Space) && !isDashing) //movement in jumping
            {
                if (jumpTimer < maxJumpDuration)
                {
                    if (wallJumpCooldownActive && (!isGrounded || remainDoubleJump))
                    {
                        if (isWallOnRight || jumpedFromRight)
                        {
                            velocity = new Vector2(-wallJumpBoost * moveSpeed, (2) * jumpForce);
                        }
                        else
                        {
                            velocity = new Vector2(wallJumpBoost * moveSpeed, (2) * jumpForce);
                        }
                    }
                    else
                    {
                        float horizontalVelocity = horizontalInput * moveSpeed;
                        if (isWallOnRight)
                        {
                            velocity = new Vector2(Mathf.Min(horizontalVelocity, 0f), (2) * jumpForce);
                        }
                        else if (isWallOnLeft)
                        {
                            velocity = new Vector2(Mathf.Max(horizontalVelocity, 0f), (2) * jumpForce);
                        }
                        else
                        {
                            velocity = new Vector2(horizontalVelocity, (2) * jumpForce);
                        }
                    }
                    jumpTimer += Time.deltaTime;
                }
                else
                {
                    keepJumping = false; // Deactivate jumping after 1 second
                }
            }
            else if (canDash && isDashing && (dashCooldownTimer <= dashCooldown))
            {
                velocity = new Vector2(rb.velocity.x + (dashForce * facingX), 0);
            }
            else //normal walking
            {
                float horizontalVelocity = horizontalInput * moveSpeed;
                if (isWallOnRight)
                {
                    velocity = new Vector2(Mathf.Min(horizontalVelocity, 0f), rb.velocity.y);
                }
                else if (isWallOnLeft)
                {
                    velocity = new Vector2(Mathf.Max(horizontalVelocity, 0f), rb.velocity.y);
                }
                else
                {
                    Vector2 movement = new Vector2(horizontalVelocity, rb.velocity.y);
                    velocity = movement;
                }
                if ((horizontalVelocity != 0) && isGrounded)
                {
                    if(!normalWalking.isPlaying)
                    {
                        normalWalking.Play();
                    }
                }
                else
                {
                    // Debug.Log("isGrounded" + isGrounded + " // HorizontalVelocity" + horizontalVelocity);
                    normalWalking.Stop();
                }
            }

            //oiasohafaousghoagewhwewenhhenjerhjhhjiowhgowjhneogjwbneojbhgwiebgijuwbebgwouebgwiouebhgiwueboiasohafaousghoagewhwewenhhenjerhjhhjiowhgowjhneogjwbneojbhgwiebgijuwbebgwouebgwiouebhgiwueb
            if (moveJustX)
            {
                velocity.y = knockBackVelocity.y;
            }
            
            rb.velocity = velocity;

            // Handle wall jump cooldown
            if (wallJumpCooldownActive)
            {
                timeSinceWallJump += Time.deltaTime;
                if (timeSinceWallJump >= wallJumpCooldown)
                {
                    wallJumpCooldownActive = false;
                }
            }

            if (!activateMovement || moveJustX)
            {
                timeSinceKnockBack += Time.deltaTime;
                if (timeSinceKnockBack >= knockBackCooldown)
                {
                    activateMovement = true;
                    moveJustX = false;
                    timeSinceKnockBack = 0f;
                }
            }
        }
        else
        {
            // Debug.Log("rb velocity es 0");
            rb.gravityScale = 0f;
            rb.velocity = Vector2.zero;
        }
    }

    public void KnockBack(Vector2 pushDirection, float pushForce)
    {
        // Debug.Log("FUNCIONAA");
        // Cancel out the current velocity to ensure accurate pushing
        rb.velocity = Vector2.zero;

        pushDirectionPlayerX = Mathf.Sign(pushDirection.x);
        pushDirectionPlayerY = Mathf.Sign(pushDirection.y);
        pushForcePlayer = pushForce;
        // Apply the force to push the enemy
        rb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);

        // Debug.Log("pushdirectionX: " + pushDirection.x);
        // Debug.Log("pushdirection: " + Vector2.up);

        if (pushDirection.x != 0)
        {
            knockBackVelocity = new Vector2(-pushDirectionPlayerX * Mathf.Abs(rb.velocity.x) *3 /*+ pushForcePlayer*/,rb.velocity.y);
            activateMovement = false;
        }
        else 
        {
            // Debug.Log("pushdirectionY: " + pushDirectionPlayerY);
            knockBackVelocity = new Vector2(rb.velocity.x,-pushDirectionPlayerY * Mathf.Abs(rb.velocity.y) *3 /*+ pushForcePlayer*/);
            moveJustX = true;
        }
    }

    public void ActivateDoubleJump(bool activate)
    {
        canDoubleJump = activate;
    }

    public void ActivateDash(bool activate)
    {
        canDash = activate;
    }

    public void ActivateWalljump(bool activate)
    {
        canWallJumpAndSlide = activate;
    }

    public void ActivateRedKey(bool activate)
    {
        hasRedKey = activate;
    }

    public void ActivateGreenKey(bool activate)
    {
        hasGreenKey = activate;
    }

    public void ActivateBlueKey(bool activate)
    {
        hasBlueKey = activate;
    }

    public void ActivateFinal(bool activate)
    {
        elapsedTime = Time.time - startTime;
        if (elapsedTime < bestTime)
        {
            bestTime = elapsedTime;
        }
        gameOver = activate;
        Debug.Log("CONGRATULATIONS: YOU HAVE SUCCESFULLY COMPLETED THE GAME");
        // Debug.Log("Paso 2: se activa la variable del Game Over");
        moveTimer = -5f;
    }

    public void NotPaused(bool activate)
    {
        canMove = activate;
        notPaused = activate;
        // Debug.Log("Es pot moure? " + canMove);
    }

    public void CanMove(bool activate)
    {
        canMove = activate;
        // Debug.Log("Es pot moure? " + canMove);
    }

    public void CoinCount()
    {
        coinCount++;
        Debug.Log("Coins: " + coinCount);
        // Debug.Log("Es pot moure? " + canMove);
    }
}
