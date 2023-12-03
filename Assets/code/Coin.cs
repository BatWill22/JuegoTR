using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Vector3 initialPosition;
    private Vector3 hiddenCoinItemPosition;
    public PlayerMovement playerMovement;

    private void Start()
    {
        // Save the initial position when the coin is instantiated
        initialPosition = transform.position;
        hiddenCoinItemPosition = new Vector3(-200, -20, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider that entered the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Increment the coin count
            other.GetComponent<PlayerMovement>().CoinCount();

            // Save the coin count (you can use PlayerPrefs or another method to persist it)

            transform.position = hiddenCoinItemPosition;
        }
    }

    public void Respawn()
    {
        // Reset the coin's position to the initial position
        transform.position = initialPosition;
    }
}
