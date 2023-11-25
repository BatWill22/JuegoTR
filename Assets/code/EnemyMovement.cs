using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    private bool moveRight = true;
    // Add other necessary variables and functions.
    private Rigidbody2D rb;
    private Collider2D enemyCollider;

    public LayerMask groundLayer;
    public LayerMask wallLayer;

    private bool isTouchingGround = true;
    private bool isCheckGrounded = false;
    private bool activateMovement = true;
    // private bool isTouchingWall = false;
    private bool isWallOnRight = false;
    private bool isWallOnLeft = false;

    private float groundTimer = 0f;
    private float maxGroundTime = 0.5f;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        //Calcular si isGrounded
        // Calculate positions for raycasts
        Vector3 rightRaycastOrigin = transform.position + new Vector3(enemyCollider.bounds.extents.x, 0f, 0f);
        Vector3 leftRaycastOrigin = transform.position - new Vector3(enemyCollider.bounds.extents.x, 0f, 0f);
        Vector3 centerRaycastOrigin = transform.position;
        // Perform raycasts
        bool rightRaycastHit = Physics2D.Raycast(rightRaycastOrigin, Vector2.down, 1f, wallLayer);
        bool leftRaycastHit = Physics2D.Raycast(leftRaycastOrigin, Vector2.down, 1f, wallLayer);
        bool centerRaycastHit = Physics2D.Raycast(centerRaycastOrigin, Vector2.down, 1f, wallLayer);
        // Check if any of the raycasts hit the ground
        isTouchingGround = enemyCollider.IsTouchingLayers(groundLayer) || (enemyCollider.IsTouchingLayers(wallLayer) && (rightRaycastHit && leftRaycastHit && centerRaycastHit || isCheckGrounded));

        isWallOnRight = enemyCollider.IsTouchingLayers(wallLayer) && Physics2D.Raycast(transform.position, Vector2.right, enemyCollider.bounds.extents.x + 0.05f, wallLayer);
        isWallOnLeft = enemyCollider.IsTouchingLayers(wallLayer) && Physics2D.Raycast(transform.position, Vector2.left, enemyCollider.bounds.extents.x + 0.05f, wallLayer);
        // isTouchingWall = isWallOnLeft || isWallOnRight;
        // if (Physics2D.Raycast(centerRaycastOrigin, Vector2.up, 1f, wallLayer) && !Physics2D.Raycast(transform.position, Vector2.right, 1f, wallLayer) && !Physics2D.Raycast(transform.position, Vector2.left, 1f, wallLayer ))
        // {
        //     isTouchingWall = false;
        //     //Debug.Log("grounded: " + isGrounded);
        // }


        // Check for changes in the environment
        if ((!isTouchingGround || isWallOnLeft || isWallOnRight) && (groundTimer <= 0f))
        {
            // Change direction
            moveRight = !moveRight;
            if (!isTouchingGround)
            {
                groundTimer = maxGroundTime;
            }
            // Flip the enemy's sprite or model to match the new direction.
            // You may need to handle this based on your enemy's setup.
        }

        if (groundTimer > 0f)
        {
            groundTimer -= Time.deltaTime;
        }

        // Move in the updated direction
        if (moveRight && activateMovement)
        {
            rb.velocity = new Vector2(+moveSpeed, rb.velocity.y);
        }
        else if (activateMovement)
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        }
        //Debug.Log("grounded: " + isTouchingGround);
        if (rb.velocity == Vector2.zero)
        {
            activateMovement = true;
        }
    }

    public void PushBack(Vector2 pushDirection, float pushForce)
    {
        Debug.Log("HOLAAAAAAAAAA");
        // Cancel out the current velocity to ensure accurate pushing
        rb.velocity = Vector2.zero;

        // Apply the force to push the enemy
        rb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
        activateMovement = false;
    }
}
