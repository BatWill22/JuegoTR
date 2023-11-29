using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashItemScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider that entered the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Call ActivateDoubleJump function from PlayerMovement script
            other.GetComponent<PlayerMovement>().ActivateDash(true);

            // Destroy the item GameObject
            Destroy(gameObject);
        }
    }
}
