using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpItemScript : MonoBehaviour
{
    private Vector3 initialDoubleJumpItemPosition;

    public GameObject doubleJumpItem;

    public PlayerMovement playerMovement;

    private Vector3 hiddenDoubleJumpItemPosition;

    private void Start()
    {
        initialDoubleJumpItemPosition = transform.position;
        hiddenDoubleJumpItemPosition = new Vector3(-200, -20, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider that entered the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Call ActivateDoubleJump function from PlayerMovement script
            other.GetComponent<PlayerMovement>().ActivateDoubleJump(true);
            // Destroy the item GameObject
            // Destroy(gameObject);
            // doubleJumpItem.SetActive(false);
            doubleJumpItem.transform.position = hiddenDoubleJumpItemPosition;
        }
    }

    public void Respawn()
    {
        // Debug.Log("entra en el respawn del item");
        // Respawn the DoubleJump item if a reference exists
        if (doubleJumpItem != null)
        {
            doubleJumpItem.SetActive(true);
            // You might want to set its position to the initial spawn position
            doubleJumpItem.transform.position = initialDoubleJumpItemPosition;
            if (playerMovement != null)
            {
                // Call ActivateDoubleJump function from PlayerMovement script
                playerMovement.ActivateDoubleJump(false);
            }
            // Debug.Log("DoubleJump item respawned! jbaeuoebngtoqbhaweughoiaoghuaheihjyopashia0opeghuohgpaiheoughoanepighaouge");
        }
    }
}
