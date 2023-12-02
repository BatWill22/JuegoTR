using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalItemScript : MonoBehaviour
{
    // private Vector3 initialFinalItemPosition;

    public GameObject finalItem;

    public PlayerHealth playerHealth;

    // private Vector3 hiddenFinalItemPosition;

    // private void Start()
    // {
    //     initialFinalItemPosition = transform.position;
    //     hiddenFinalItemPosition = new Vector3(-200, -20, 0);
    // }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider that entered the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Call ActivateFinal function from PlayerMovement script
            // Debug.Log("Paso 1: el jugador toca el objeto final");
            other.GetComponent<PlayerHealth>().ActivateFinal(true);
            // Destroy the item GameObject
            // Destroy(gameObject);
            // finalItem.SetActive(false);
            // finalItem.transform.position = hiddenFinalItemPosition;
        }
    }

    // public void Respawn()
    // {
    //     // Debug.Log("entra en el respawn del item");
    //     // Respawn the Final item if a reference exists
    //     if (finalItem != null)
    //     {
    //         // finalItem.SetActive(true);
    //         // You might want to set its position to the initial spawn position
    //         // finalItem.transform.position = initialFinalItemPosition;
    //         if (playerHealth != null)
    //         {
    //             // Call ActivateFinal function from PlayerMovement script
    //             playerHealth.ActivateFinal(false);
    //         }
    //         // Debug.Log("Final item respawned! jbaeuoebngtoqbhaweughoiaoghuaheihjyopashia0opeghuohgpaiheoughoanepighaouge");
    //     }
    // }
}
