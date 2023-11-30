using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedKeyItemScript : MonoBehaviour
{
    private Vector3 initialRedKeyItemPosition;

    public GameObject redKeyItem;

    public PlayerMovement playerMovement;

    private Vector3 hiddenRedKeyItemPosition;

    private void Start()
    {
        initialRedKeyItemPosition = transform.position;
        hiddenRedKeyItemPosition = new Vector3(-200, -20, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider that entered the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Call ActivateDoubleJump function from PlayerMovement script
            other.GetComponent<PlayerMovement>().ActivateRedKey(true);
            // Destroy the item GameObject
            // Destroy(gameObject);
            // redKeyItem.SetActive(false);
            redKeyItem.transform.position = hiddenRedKeyItemPosition;
        }
    }

    public void Respawn()
    {
        Debug.Log("entra en el respawn del item");
        // Respawn the RedKey item if a reference exists
        if (redKeyItem != null)
        {
            redKeyItem.SetActive(true);
            // You might want to set its position to the initial spawn position
            redKeyItem.transform.position = initialRedKeyItemPosition;
            if (playerMovement != null)
            {
                // Call ActivateDoubleJump function from PlayerMovement script
                playerMovement.ActivateRedKey(false);
            }
            Debug.Log("RedKey item respawned! jbaeuoebngtoqbhaweughoiaoghuaheihjyopashia0opeghuohgpaiheoughoanepighaouge");
        }
    }
}
