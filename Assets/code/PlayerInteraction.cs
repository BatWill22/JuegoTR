using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding object is an enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Get the relative position of the enemy
            Vector3 relativePosition = collision.transform.position - transform.position;

            // Determine the direction based on the relative position
            Vector2 pushDirection = DeterminePushDirection(relativePosition);
            
            // Set the force value based on your game's logic
            float pushForce = 5;

            // Call the EnemyInteraction function on the PlayerHealth script
            GetComponent<PlayerHealth>().EnemyInteraction(pushDirection, pushForce);

            EnemyMovement enemyMovement = collision.gameObject.GetComponent<EnemyMovement>();

            // Check if the reference is not null
            if (enemyMovement != null)
            {
                // Call the ChangeDirection function
                enemyMovement.ChangeDirection();
            }
            Debug.Log("Player took damage. Current health: ");
        }
    }

    // Determine the push direction based on the relative position
    private Vector2 DeterminePushDirection(Vector3 relativePosition)
    {
        float x = relativePosition.x;
        float y = relativePosition.y;

        // Check if the enemy is on the right, left, up, or down
        if (Mathf.Abs(x) > Mathf.Abs(y))
        {
            return (x > 0) ? Vector2.right : Vector2.left;
        }
        else
        {
            return (y > 0) ? Vector2.up : Vector2.down;
        }
    }
}
