using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Public variables from another script
    // public int facingX; // -1 for left, 0 for none, 1 for right
    // public int facingY; // -1 for down, 0 for none, 1 for up

    // Add reference to your Animator component
    // private Animator animator;
    
    private float attackRange = 3f; // Set your attack range here

    int facingX;
    int facingY;


    public PlayerMovement playerMovement;
    public LayerMask enemyLayer;

    private bool canAttack = true; // Variable to track if the player can currently attack

    // Adjust this value based on your desired cooldown time (in seconds)
    public float attackCooldown = 0.75f;

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
        }

        // Check for the attack input, e.g., you might use the spacebar
        if (Input.GetButtonDown("Fire1") && canAttack)
        {
            // Trigger the attack animation
            // animator.SetTrigger("Attack");

            // Play attack sound effect
            // if (attackSound != null)
            // {
            //     AudioSource.PlayClipAtPoint(attackSound, transform.position);
            // }

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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, attackRange, enemyLayer);
        
        // Adjust this force value based on your game's requirements
        float pushForce = 3.5f;

        // Calculate the direction to push the enemy
        Vector2 pushDirection = Vector2.right;

        playerMovement.KnockBack(pushDirection, pushForce);

        if (hit.collider != null)
        {
            // Enemy detected, push them back
            EnemyMovement enemy = hit.collider.GetComponent<EnemyMovement>();

            if (enemy != null)
            {
                // Apply force to the enemy
                enemy.PushBack(pushDirection, pushForce);
            }
        }
    }

    private void AttackLeft()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, attackRange, enemyLayer);
        // Adjust this force value based on your game's requirements
        float pushForce = 3.5f;

        // Calculate the direction to push the enemy
        Vector2 pushDirection = Vector2.left;
        playerMovement.KnockBack(pushDirection, pushForce);

        if (hit.collider != null)
        {
            // Enemy detected, push them back
            EnemyMovement enemy = hit.collider.GetComponent<EnemyMovement>();

            if (enemy != null)
            {
                // Apply force to the enemy
                enemy.PushBack(pushDirection, pushForce);
            }
        }
    }

    private void AttackUp()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, attackRange, enemyLayer);

        if (hit.collider != null)
        {
            // Enemy detected, push them back
            EnemyMovement enemy = hit.collider.GetComponent<EnemyMovement>();

            if (enemy != null)
            {
                // Adjust this force value based on your game's requirements
                float pushForce = 3.5f;

                // Calculate the direction to push the enemy
                Vector2 pushDirection = Vector2.up;

                // Apply force to the enemy
                enemy.PushBack(pushDirection, pushForce);
                playerMovement.KnockBack(pushDirection, pushForce);
            }
        }
    }

    private void AttackDown()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, attackRange, enemyLayer);

        if (hit.collider != null)
        {
            // Enemy detected, push them back
            EnemyMovement enemy = hit.collider.GetComponent<EnemyMovement>();

            if (enemy != null)
            {
                // Adjust this force value based on your game's requirements
                float pushForce = 3.5f;

                // Calculate the direction to push the enemy
                Vector2 pushDirection = Vector2.down;

                // Apply force to the enemy
                enemy.PushBack(pushDirection, pushForce);
                playerMovement.KnockBack(pushDirection, pushForce);
            }
        }
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
}
