using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighItemScript : MonoBehaviour
{
    // private Vector3 initialHighItemPosition;

    public GameObject highItem;

    public PlayerHealth playerHealth;

    // private Vector3 hiddenHighItemPosition;

    private void Start()
    {
        // initialHighItemPosition = transform.position;
        // hiddenHighItemPosition = new Vector3(-200, -20, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider that entered the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Call ActivateHigh function from PlayerMovement script
            other.GetComponent<PlayerHealth>().ActivateHigh(true);
            // Destroy the item GameObject
            // Destroy(gameObject);
            // highItem.SetActive(false);
            // highItem.transform.position = hiddenHighItemPosition;
        }
    }

    // public void Respawn()
    // {
    //     // Debug.Log("entra en el respawn del item");
    //     // Respawn the High item if a reference exists
    //     if (highItem != null)
    //     {
    //         highItem.SetActive(true);
    //         // You might want to set its position to the initial spawn position
    //         highItem.transform.position = initialHighItemPosition;
    //         if (playerMovement != null)
    //         {
    //             // Call ActivateHigh function from PlayerMovement script
    //             playerMovement.ActivateHigh(false);
    //         }
    //         // Debug.Log("High item respawned! jbaeuoebngtoqbhaweughoiaoghuaheihjyopashia0opeghuohgpaiheoughoanepighaouge");
    //     }
    // }
}
