using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueKeyItemScript : MonoBehaviour
{
    private Vector3 initialBlueKeyItemPosition;

    public GameObject blueKeyItem;

    public PlayerMovement playerMovement;

    private Vector3 hiddenBlueKeyItemPosition;

    private void Start()
    {
        initialBlueKeyItemPosition = transform.position;
        hiddenBlueKeyItemPosition = new Vector3(-200, -20, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider that entered the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Call ActivateDoubleJump function from PlayerMovement script
            other.GetComponent<PlayerMovement>().ActivateBlueKey(true);
            // Destroy the item GameObject
            // Destroy(gameObject);
            // blueKeyItem.SetActive(false);
            blueKeyItem.transform.position = hiddenBlueKeyItemPosition;
        }
    }

    public void Respawn()
    {
        // Debug.Log("entra en el respawn del item");
        // Respawn the BlueKey item if a reference exists
        if (blueKeyItem != null)
        {
            blueKeyItem.SetActive(true);
            // You might want to set its position to the initial spawn position
            blueKeyItem.transform.position = initialBlueKeyItemPosition;
            if (playerMovement != null)
            {
                // Call ActivateDoubleJump function from PlayerMovement script
                playerMovement.ActivateBlueKey(false);
            }
            // Debug.Log("BlueKey item respawned! jbaeuoebngtoqbhaweughoiaoghuaheihjyopashia0opeghuohgpaiheoughoanepighaouge");
        }
    }
}
