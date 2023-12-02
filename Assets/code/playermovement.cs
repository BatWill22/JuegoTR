using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    //Variable Declaration

    public bool canMove = false;

    public float moveSpeed = 5f;
    public float jumpForce = 3000f;
    public float wallSlideSpeed = 2f;
    public float wallJumpCooldown = 0.2f; // Adjust this cooldown duration as needed.

    public LayerMask groundLayer;
    public LayerMask wallLayer;

    public bool canWallJumpAndSlide = false; // New variable for enabling/disabling wall jump and slide
    public bool canDoubleJump = false;
    public bool canDash = false; // Can dash

    public bool hasRedKey = false; // New variable for enabling/disabling wall jump and slide
    public bool hasGreenKey = false;
    public bool hasBlueKey = false; // Can dash

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

    public float dashForce = 10f; // Dash force
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
    }

    private void Update()
    {
        
        if (moveTimer <= delayBeforeMove)
        {
            moveTimer += Time.deltaTime;
            canMove = false;
        }

        if (moveTimer >= delayBeforeMove)
        {
            if (hasBeenHit)
            {
                rb.gravityScale = 10f;
                transform.position = lastCheckGroundPosition; // Set the player's position to the last ground position
                hasBeenHit = false;
            }
            if (gameOver)
            {
                SceneManager.LoadScene("EndGameMenu");
                gameOver = false;
            }
            canMove = true;
        }

        if (canMove)
        {
            isCheckGrounded = playerCollider.IsTouchingLayers(groundLayer);
            //Calcular si isGrounded
            // Calculate positions for raycasts
            Vector3 rightRaycastOrigin = transform.position + new Vector3(playerCollider.bounds.extents.x, 0f, 0f);
            Vector3 leftRaycastOrigin = transform.position - new Vector3(playerCollider.bounds.extents.x, 0f, 0f);
            Vector3 centerRaycastOrigin = transform.position;
            // Perform raycasts
            bool rightRaycastHit = Physics2D.Raycast(rightRaycastOrigin, Vector2.down, playerCollider.bounds.extents.y + 0.05f, wallLayer);
            bool leftRaycastHit = Physics2D.Raycast(leftRaycastOrigin, Vector2.down, playerCollider.bounds.extents.y + 0.05f, wallLayer);
            bool centerRaycastHit = Physics2D.Raycast(centerRaycastOrigin, Vector2.down, playerCollider.bounds.extents.y + 0.05f, wallLayer);
            // Check if any of the raycasts hit the ground
            isGrounded = playerCollider.IsTouchingLayers(groundLayer) || (playerCollider.IsTouchingLayers(wallLayer) && (rightRaycastHit || leftRaycastHit || centerRaycastHit || isCheckGrounded));

            // Calculate positions for raycasts on Y
            // Vector3 upRaycastOrigin = transform.position + new Vector3(0f, playerCollider.bounds.extents.y, 0f);
            // Vector3 downRaycastOrigin = transform.position - new Vector3(0f, playerCollider.bounds.extents.y, 0f);
            // Vector3 centerRaycastOriginY = transform.position;
            // // Perform raycasts
            // bool upRaycastHitRight = Physics2D.Raycast(upRaycastOrigin, Vector2.right, 1f, wallLayer);
            // bool downRaycastHitRight = Physics2D.Raycast(downRaycastOrigin, Vector2.right, 1f, wallLayer);
            // bool centerRaycastHitYRight = Physics2D.Raycast(centerRaycastOriginY, Vector2.right, 1f, wallLayer);
            // bool upRaycastHitLeft = Physics2D.Raycast(upRaycastOrigin, Vector2.left, 1f, wallLayer);
            // bool downRaycastHitLeft = Physics2D.Raycast(downRaycastOrigin, Vector2.left, 1f, wallLayer);
            // bool centerRaycastHitYLeft = Physics2D.Raycast(centerRaycastOriginY, Vector2.left, 1f, wallLayer);
            //calculate if it's touching a wall and not a roof
            // isWallOnRight = playerCollider.IsTouchingLayers(wallLayer) && (upRaycastHitRight || downRaycastHitRight || centerRaycastHitYRight);
            // isWallOnRight = playerCollider.IsTouchingLayers(wallLayer) && (upRaycastHitLeft || downRaycastHitLeft || centerRaycastHitYLeft);
            isWallOnRight = playerCollider.IsTouchingLayers(wallLayer) && Physics2D.Raycast(transform.position, Vector2.right, playerCollider.bounds.extents.x + 0.05f, wallLayer);
            isWallOnLeft = playerCollider.IsTouchingLayers(wallLayer) && Physics2D.Raycast(transform.position, Vector2.left, playerCollider.bounds.extents.x + 0.05f, wallLayer);
            isTouchingWall = isWallOnLeft || isWallOnRight;
            if (Physics2D.Raycast(centerRaycastOrigin, Vector2.up, 1f, wallLayer) && !Physics2D.Raycast(transform.position, Vector2.right, 1f, wallLayer) && !Physics2D.Raycast(transform.position, Vector2.left, 1f, wallLayer ))
            {
                isTouchingWall = false;
                //Debug.Log("grounded: " + isGrounded);
            }

            //Keep Checkground Position
            if (isCheckGrounded)
            {
                lastCheckGroundPosition = transform.position;
            }

            //determine which side of the wall
            // isWallOnRight = isTouchingWall && Physics2D.Raycast(transform.position, Vector2.right, 1f, wallLayer);
            // isWallOnLeft = isTouchingWall && Physics2D.Raycast(transform.position, Vector2.left, 1f, wallLayer);

            // Activate dash
            if (canDash && Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && dashCooldownTimer <= 0)
            {
                isDashing = true;
                dashDurationTimer = dashDuration;
                dashCooldownTimer = dashCooldown;
            }

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

            if (Input.GetKeyDown(KeyCode.Space)) //When press space down
            {
                if (isGrounded)
                {
                    velocity = new Vector2(rb.velocity.x, jumpForce);

                    keepJumping = true;
                    jumpTimer = 0f;
                }
                else if (isWallSliding && !wallJumpCooldownActive)
                {
                    Vector2 wallJumpDirection = isWallOnRight ? Vector2.left : Vector2.right;
                    isWallSliding = false;
                    jumpedFromRight = isWallOnRight;
                    velocity = wallJumpDirection * jumpForce;
                    wallJumpCooldownActive = true;
                    timeSinceWallJump = 0f;
                    keepJumping = true;
                    jumpTimer = 0f;
                }
                else if (canDoubleJump && remainDoubleJump)
                {
                    if (wallJumpCooldownActive)
                    {
                        wallJumpCooldownActive = false;
                    }
                    remainDoubleJump = false;
                    velocity = new Vector2(rb.velocity.x, jumpForce);
                    keepJumping = true;
                    jumpTimer = 0f;
                }
                rb.velocity = velocity;
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

            float horizontalInput = Input.GetAxisRaw("Horizontal");

            if (!activateMovement)
            {
                // Debug.Log(pushDirectionPlayer);
                // Debug.Log("velocitat:"+rb.velocity.x); 
                velocity = knockBackVelocity;
            }
            else if (isTouchingWall && !isGrounded && isWallSliding && !wallJumpCooldownActive && !isDashing) //movement in wallsliding
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

            if (Input.GetKeyUp(KeyCode.Space)) // Stop jumping when stop pressing space
            {
                keepJumping = false;
            }
            if ((isGrounded || isWallSliding) && canDoubleJump) //allow double jump when touching ground/wall
            {
                remainDoubleJump = true;
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
        gameOver = activate;
        Debug.Log("CONGRATULATIONS: YOU HAVE SUCCESFULLY COMPLETED THE GAME");
        // Debug.Log("Paso 2: se activa la variable del Game Over");
        moveTimer = -5f;
    }
}
