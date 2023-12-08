using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public PlayerHealth playerHealth;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            // Call the TakeDamage function with the desired damage amount
            playerHealth.TakeDamage(1);
            int health = playerHealth.currentHealth;
            int maxHealth = playerHealth.maxHealth;
            if (collision.gameObject.CompareTag("Player") && (health != 0) && (health != maxHealth))
            {
                PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
                if (player != null)
                {
                    animator.SetTrigger("Die");
                    player.ResetToLastCheckZonePosition(); // Reset to the last ground position
                }
            }
        }
    }
}
