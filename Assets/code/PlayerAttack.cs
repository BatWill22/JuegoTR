using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Public variables from another script
    // public int facingX; // -1 for left, 0 for none, 1 for right
    // public int facingY; // -1 for down, 0 for none, 1 for up

    // Add reference to your Animator component
    // private Animator animator;
    [SerializeField] private Animator animator;
    
    private float attackRange = 2f; // Set your attack range here

    int facingX;
    int facingY;

    public RedDoorObjectScript redDoor;
    public GreenDoorObjectScript greenDoor;
    public BlueDoorObjectScript blueDoor;

    public FinalDoorObjectScript finalDoor;
    public ExitDoorObjectScript exitDoor;

    public PlayerMovement playerMovement;
    private EnemyHealth enemyHealth;
    public Collider2D playerCollider;
    public LayerMask enemyLayer;
    public LayerMask obstacleLayer;
    public LayerMask doorLayer;

    private bool canAttack = true; // Variable to track if the player can currently attack

    // Adjust this value based on your desired cooldown time (in seconds)
    public float attackCooldown = 0.75f;

    private bool moves = false;

    [Header("Sounds")]
    [SerializeField] private AudioSource attackSound;
    [SerializeField] private AudioSource openDoorSound;

    // Add reference to your attack sound effect
    // public AudioClip attackSound;

    // private void Start()
    // {
    //     // Get the Animator component on the same GameObject
    //     animator = GetComponent<Animator>();
    // }

    private void Update()
    {
        if (playerMovement != null)
        {
            facingX = playerMovement.facingX;
            facingY = playerMovement.facingY;
            // Debug.Log("facingX: " + facingX);
            // Debug.Log("facingY: " + facingY);
            moves = playerMovement.canMove;
        }

        // Check for the attack input, e.g., you might use the spacebar
        if (Input.GetButtonDown("Fire1") && canAttack && moves)
        {
            // Trigger the attack animation
            // animator.SetTrigger("Attack");

            if (attackSound != null)
            {
                attackSound.Play();
            }

            // Perform the attack logic based on the facing direction
            if (facingY == 1)
            {
                AttackUp();
            }
            else if (facingY == -1)
            {
                AttackDown();
            }
            else if (facingX == 1)
            {
                AttackRight();
            }
            else if (facingX == -1)
            {
                AttackLeft();
            }
            StartCoroutine(AttackCooldown());
        }
    }

    private void AttackRight()
    {
        animator.SetTrigger("Attack");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, attackRange, enemyLayer);
        
        // Adjust this force value based on your game's requirements
        float pushForce = 3.5f;

        // Calculate the direction to push the enemy
        Vector2 pushDirection = Vector2.right;

        // playerMovement.KnockBack(pushDirection, pushForce);

        if (hit.collider != null)
        {
            // Enemy detected, push them back
            EnemyMovement enemy = hit.collider.GetComponent<EnemyMovement>();
            EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();

            if (enemy != null)
            {
                // Apply force to the enemy
                enemy.PushBack(pushDirection, pushForce);
                if (enemyHealth != null)
                {
                    enemyHealth.GetHit();
                }
                playerMovement.KnockBack(pushDirection, pushForce);
            }
        }

        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, Vector2.right, attackRange, obstacleLayer);
        if (hit2.collider != null)
        {
            playerMovement.KnockBack(pushDirection, pushForce);
        }

        RaycastHit2D hit3 = Physics2D.Raycast(transform.position, Vector2.right, attackRange, doorLayer);
        if (hit3.collider != null)
        {
            NormalDoorObjectScript normalDoor = hit3.collider.GetComponent<NormalDoorObjectScript>();
            // Debug.Log("hit3.collider: " + hit3.collider);
            if (normalDoor != null)
            {
                // Debug.Log("normalDoor is not null");
                normalDoor.OpenDoor();
                // Debug.Log("Opened the door");
            }
            
            // Assuming each door has a unique tag
            string doorTag = hit3.collider.tag;
            // Debug.Log("hit3.collider no es null:" + doorTag);

            switch (doorTag)
            {
                case "RedDoor":
                    RedDoorObjectScript redDoor = hit3.collider.GetComponent<RedDoorObjectScript>();
                    if (redDoor != null)
                    {
                        bool redKey = PlayerMovement.hasRedKey;
                        if (redKey)
                        {
                            redDoor.OpenDoor();
                        }
                    }
                    break;

                case "GreenDoor":
                    GreenDoorObjectScript greenDoor = hit3.collider.GetComponent<GreenDoorObjectScript>();
                    if (greenDoor != null)
                    {
                        bool greenKey = PlayerMovement.hasGreenKey;
                        if (greenKey)
                        {
                            greenDoor.OpenDoor();
                        }
                    }
                    break;

                case "BlueDoor":
                    BlueDoorObjectScript blueDoor = hit3.collider.GetComponent<BlueDoorObjectScript>();
                    if (blueDoor != null)
                    {
                        bool blueKey = PlayerMovement.hasBlueKey;
                        if (blueKey)
                        {
                            blueDoor.OpenDoor();
                        }
                    }
                    break;

                case "FinalDoor":
                    FinalDoorObjectScript finalDoor = hit3.collider.GetComponent<FinalDoorObjectScript>();
                    if (finalDoor != null)
                    {
                        finalDoor.OpenDoor();
                        ExitDoorObjectScript exitDoor = FindObjectOfType<ExitDoorObjectScript>();
                        if (exitDoor != null)
                        {
                            // Debug.Log("DEVUELVE ITEM WALJUM");
                            exitDoor.Respawn();
                        }
                    }
                    break;

                // case "NormalDoor":
                //     NormalDoorObjectScript normalDoor = hit3.collider.GetComponent<NormalDoorObjectScript>();
                //     if (normalDoor != null)
                //     {
                //         normalDoor.OpenDoor();
                //     }
                //     break;

                default:
                    // Handle other cases or do nothing
                    break;
            }
        }

        // show raycast in the scene view
        Debug.DrawRay(transform.position, Vector2.right * attackRange, (hit.collider || hit2.collider || hit3.collider) ? Color.red : Color.green, 0.1f);

    }

    private void AttackLeft()
    {
        animator.SetTrigger("Attack");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, attackRange, enemyLayer);

        // show raycast in the scene view
        // Debug.DrawRay(transform.position, Vector2.left * attackRange, hit.collider ? Color.red : Color.green, 0.1f);

        // Adjust this force value based on your game's requirements
        float pushForce = 3.5f;

        // Calculate the direction to push the enemy
        Vector2 pushDirection = Vector2.left;
        // playerMovement.KnockBack(pushDirection, pushForce);

        if (hit.collider != null)
        {
            // Enemy detected, push them back
            EnemyMovement enemy = hit.collider.GetComponent<EnemyMovement>();
            EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();

            if (enemy != null)
            {
                // Apply force to the enemy
                enemy.PushBack(pushDirection, pushForce);
                if (enemyHealth != null)
                {
                    enemyHealth.GetHit();
                }
                playerMovement.KnockBack(pushDirection, pushForce);
            }
        }

        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, Vector2.left, attackRange, obstacleLayer);
        if (hit2.collider != null)
        {
            playerMovement.KnockBack(pushDirection, pushForce);
        }

        RaycastHit2D hit3 = Physics2D.Raycast(transform.position, Vector2.left, attackRange, doorLayer);
        if (hit3.collider != null)
        {
            // Assuming each door has a unique tag
            
            NormalDoorObjectScript normalDoor = hit3.collider.GetComponent<NormalDoorObjectScript>();
            // Debug.Log("hit3.collider: " + hit3.collider);
            if (normalDoor != null)
            {
                // Debug.Log("normalDoor is not null");
                normalDoor.OpenDoor();
                // Debug.Log("Opened the door");
            }
            
            string doorTag = hit3.collider.tag;
            // Debug.Log("hit3.collider no es null:" + doorTag);

            switch (doorTag)
            {
                case "RedDoor":
                    RedDoorObjectScript redDoor = hit3.collider.GetComponent<RedDoorObjectScript>();
                    if (redDoor != null)
                    {
                        bool redKey = PlayerMovement.hasRedKey;
                        if (redKey)
                        {
                            redDoor.OpenDoor();
                        }
                    }
                    break;

                case "GreenDoor":
                    GreenDoorObjectScript greenDoor = hit3.collider.GetComponent<GreenDoorObjectScript>();
                    if (greenDoor != null)
                    {
                        bool greenKey = PlayerMovement.hasGreenKey;
                        if (greenKey)
                        {
                            greenDoor.OpenDoor();
                        }
                    }
                    break;

                case "BlueDoor":
                    BlueDoorObjectScript blueDoor = hit3.collider.GetComponent<BlueDoorObjectScript>();
                    if (blueDoor != null)
                    {
                        bool blueKey = PlayerMovement.hasBlueKey;
                        if (blueKey)
                        {
                            blueDoor.OpenDoor();
                        }
                    }
                    break;

                case "FinalDoor":
                    FinalDoorObjectScript finalDoor = hit3.collider.GetComponent<FinalDoorObjectScript>();
                    if (finalDoor != null)
                    {
                        finalDoor.OpenDoor();
                        ExitDoorObjectScript exitDoor = FindObjectOfType<ExitDoorObjectScript>();
                        if (exitDoor != null)
                        {
                            // Debug.Log("oioearta fubak eadag ritor");
                            exitDoor.OpenDoor();
                        }
                    }
                    break;

                default:
                    // Handle other cases or do nothing
                    break;
            }
        }

        // show raycast in the scene view
        Debug.DrawRay(transform.position, Vector2.left * attackRange, (hit.collider || hit2.collider || hit3.collider) ? Color.red : Color.green, 0.1f);

    }

    private void AttackUp()
    {
        animator.SetTrigger("AttackUp");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 2*attackRange, enemyLayer);

        // Adjust this force value based on your game's requirements
        float pushForce = 3.5f;

        // Calculate the direction to push the enemy
        Vector2 pushDirection = Vector2.up;

        if (hit.collider != null)
        {
            // Enemy detected, push them back
            EnemyMovement enemy = hit.collider.GetComponent<EnemyMovement>();
            EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();

            if (enemy != null)
            {
                // Apply force to the enemy
                enemy.PushBack(pushDirection, pushForce);
                if (enemyHealth != null)
                {
                    enemyHealth.GetHit();
                }
                playerMovement.KnockBack(pushDirection, pushForce);
            }
        }

        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, Vector2.right, attackRange, obstacleLayer);
        if (hit2.collider != null)
        {
            playerMovement.KnockBack(pushDirection, pushForce);
        }

        // show raycast in the scene view
        Debug.DrawRay(transform.position, Vector2.up * attackRange, (hit.collider || hit2.collider) ? Color.red : Color.green, 0.1f);

    }

    private void AttackDown()
    {
        animator.SetTrigger("AttackDown");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 2.5f*attackRange, enemyLayer);

        // Adjust this force value based on your game's requirements
        float pushForce = 3.5f;

        // Calculate the direction to push the enemy
        Vector2 pushDirection = Vector2.down;

        if (hit.collider != null)
        {
            // Enemy detected, push them back
            EnemyMovement enemy = hit.collider.GetComponent<EnemyMovement>();
            EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();

            if (enemy != null)
            {
                // Apply force to the enemy
                enemy.PushBack(pushDirection, pushForce);
                if (enemyHealth != null)
                {
                    enemyHealth.GetHit();
                }
                playerMovement.KnockBack(pushDirection, pushForce);
            }
        }

        Vector3 rightRaycastOrigin = transform.position + new Vector3(playerCollider.bounds.extents.x, 0f, 0f);
        Vector3 leftRaycastOrigin = transform.position - new Vector3(playerCollider.bounds.extents.x, 0f, 0f);
        Vector3 centerRaycastOrigin = transform.position;

        // Vector3 rightRaycastOrigin2 = transform.position + new Vector3(playerCollider.bounds.extents.x*1.75, 0f, 0f);
        // Vector3 leftRaycastOrigin2 = transform.position - new Vector3(playerCollider.bounds.extents.x*1.75, 0f, 0f);

        // Perform raycasts
        bool rightRaycastHit = Physics2D.Raycast(rightRaycastOrigin, Vector2.down, attackRange, obstacleLayer);
        bool leftRaycastHit = Physics2D.Raycast(leftRaycastOrigin, Vector2.down, attackRange, obstacleLayer);
        bool centerRaycastHit = Physics2D.Raycast(centerRaycastOrigin, Vector2.down, attackRange, obstacleLayer);

        // bool rightRaycastHit2 = Physics2D.Raycast(rightRaycastOrigin2, Vector2.down, attackRange, obstacleLayer);
        // bool leftRaycastHit2 = Physics2D.Raycast(leftRaycastOrigin2, Vector2.down, attackRange, obstacleLayer);

        if (/*rightRaycastHit2 || leftRaycastHit2 ||*/ rightRaycastHit || leftRaycastHit || centerRaycastHit)
        {
            // Debug.Log("hit2.collider: ");
            playerMovement.KnockBack(pushDirection, pushForce);
        }

        // RaycastHit2D hit2 = Physics2D.Raycast(transform.position, Vector2.right, attackRange, obstacleLayer);
        // Debug.DrawRay(transform.position, Vector2.right * attackRange, Color.red, 0.1f);
        // Debug.Log("hit2.collider: " + hit2.collider);
        // if (hit2.collider != null)
        // {
        //     playerMovement.KnockBack(pushDirection, pushForce);
        // }

        // show raycast in the scene view   ESTOS DE AQUI SON PARA VER LOS RAYCASTS EN PANTALLA CON COLOR VERDE O ROJO
        Debug.DrawRay(transform.position, Vector2.down * attackRange, (hit.collider || centerRaycastHit) ? Color.red : Color.green, 0.1f);
        Debug.DrawRay(rightRaycastOrigin, Vector2.down * attackRange, rightRaycastHit ? Color.red : Color.green, 0.1f);
        Debug.DrawRay(leftRaycastOrigin, Vector2.down * attackRange, leftRaycastHit ? Color.red : Color.green, 0.1f);
    }

    private IEnumerator AttackCooldown()
    {
        // Set canAttack to false to prevent further attacks during cooldown
        canAttack = false;

        // Wait for the specified cooldown duration
        yield return new WaitForSeconds(attackCooldown);

        // Reset canAttack to true after cooldown
        canAttack = true;
    }

    public void CanAttack(bool activate)
    {
        canAttack = activate;
        // Debug.Log("Es pot moure? " + canMove);
    }
}
