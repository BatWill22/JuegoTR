using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenKeyItemScript : MonoBehaviour
{
    private Vector3 initialGreenKeyItemPosition;

    public GameObject greenKeyItem;

    public PlayerMovement playerMovement;

    private Vector3 hiddenGreenKeyItemPosition;

    private void Start()
    {
        initialGreenKeyItemPosition = transform.position;
        hiddenGreenKeyItemPosition = new Vector3(-200, -20, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider that entered the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Call ActivateDoubleJump function from PlayerMovement script
            other.GetComponent<PlayerMovement>().ActivateGreenKey(true);
            // Destroy the item GameObject
            // Destroy(gameObject);
            // greenKeyItem.SetActive(false);
            greenKeyItem.transform.position = hiddenGreenKeyItemPosition;
        }
    }

    public void Respawn()
    {
        Debug.Log("entra en el respawn del item");
        // Respawn the GreenKey item if a reference exists
        if (greenKeyItem != null)
        {
            greenKeyItem.SetActive(true);
            // You might want to set its position to the initial spawn position
            greenKeyItem.transform.position = initialGreenKeyItemPosition;
            if (playerMovement != null)
            {
                // Call ActivateDoubleJump function from PlayerMovement script
                playerMovement.ActivateGreenKey(false);
            }
            Debug.Log("GreenKey item respawned! jbaeuoebngtoqbhaweughoiaoghuaheihjyopashia0opeghuohgpaiheoughoanepighaouge");
        }
    }
}
