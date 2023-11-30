using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashItemScript : MonoBehaviour
{
    private Vector3 initialDashItemPosition;

    public GameObject dashItem;

    public PlayerMovement playerMovement;

    private Vector3 hiddenDashItemPosition;

    private void Start()
    {
        initialDashItemPosition = transform.position;
        hiddenDashItemPosition = new Vector3(-200, -20, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider that entered the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Call ActivateDoubleJump function from PlayerMovement script
            other.GetComponent<PlayerMovement>().ActivateDash(true);
            // Destroy the item GameObject
            // Destroy(gameObject);
            // dashItem.SetActive(false);
            dashItem.transform.position = hiddenDashItemPosition;
        }
    }

    public void Respawn()
    {
        Debug.Log("entra en el respawn del item");
        // Respawn the Dash item if a reference exists
        if (dashItem != null)
        {
            dashItem.SetActive(true);
            // You might want to set its position to the initial spawn position
            dashItem.transform.position = initialDashItemPosition;
            if (playerMovement != null)
            {
                // Call ActivateDoubleJump function from PlayerMovement script
                playerMovement.ActivateDash(false);
            }
            Debug.Log("Dash item respawned! jbaeuoebngtoqbhaweughoiaoghuaheihjyopashia0opeghuohgpaiheoughoanepighaouge");
        }
    }
}
