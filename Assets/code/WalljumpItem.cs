using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalljumpItemScript : MonoBehaviour
{
    private Vector3 initialWalljumpItemPosition;

    public GameObject walljumpItem;

    public PlayerMovement playerMovement;

    private Vector3 hiddenWalljumpItemPosition;

    private void Start()
    {
        initialWalljumpItemPosition = transform.position;
        hiddenWalljumpItemPosition = new Vector3(-200, -20, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider that entered the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Call ActivateWalljump function from PlayerMovement script
            other.GetComponent<PlayerMovement>().ActivateWalljump(true);
            // Destroy the item GameObject
            // Destroy(gameObject);
            // walljumpItem.SetActive(false);
            walljumpItem.transform.position = hiddenWalljumpItemPosition;
        }
    }

    public void Respawn()
    {
        // Debug.Log("entra en el respawn del item");
        // Respawn the Walljump item if a reference exists
        if (walljumpItem != null)
        {
            walljumpItem.SetActive(true);
            // You might want to set its position to the initial spawn position
            walljumpItem.transform.position = initialWalljumpItemPosition;
            if (playerMovement != null)
            {
                // Call ActivateWalljump function from PlayerMovement script
                playerMovement.ActivateWalljump(false);
            }
            // Debug.Log("Walljump item respawned! jbaeuoebngtoqbhaweughoiaoghuaheihjyopashia0opeghuohgpaiheoughoanepighaouge");
        }
    }
}
