using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 7;
    public int currentHealth;
    public Vector2 respawnCoordinates = new Vector2(0, 1);
    private bool invincible = false;
    public float invincibilityDuration = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Public function to decrease player's health
    public void TakeDamage(int damageAmount)
    {
        if (!invincible)
        {
            currentHealth -= damageAmount;
            // Check if the player's health is below or equal to zero
            if (currentHealth <= 0)
            {
                // Player is defeated, respawn the player
                Respawn(); //hacer que si tienes las tres llaves y las tres habilidades reaparezcas en la zona final en vez de morir, a modo de unico checkpoint
            }
            else
            {
                StartCoroutine(StartInvincibility());
                Debug.Log("Player took damage. Current health: " + currentHealth);
            }
        }
    }

    // Public function to increase player's health
    // public void Heal(int healAmount)
    // {
    //     currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);

    //     Debug.Log("Player healed. Current health: " + currentHealth);
    // }

    // Respawn the player at specified coordinates
    private void Respawn() 
    {
        // Move the player to the respawn coordinates
        transform.position = new Vector3(respawnCoordinates.x, respawnCoordinates.y, transform.position.z);

        // Reset the player's health to maximum
        currentHealth = maxHealth;

        Debug.Log("Player respawned at coordinates: " + respawnCoordinates);
    }

    public void EnemyInteraction(Vector2 pushDirection, float pushForce)
    {
        // Call KnockBack from PlayerMovement script
        GetComponent<PlayerMovement>().KnockBack(pushDirection, pushForce);

        // Call TakeDamage from this script
        TakeDamage(1); // Assuming the enemy deals 1 damage, adjust as needed
    }

    private IEnumerator StartInvincibility()
    {
        invincible = true;

        // Wait for the specified duration
        yield return new WaitForSeconds(invincibilityDuration);

        // End of invincibility
        invincible = false;
    }
}
