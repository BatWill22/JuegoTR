using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsTrigger : MonoBehaviour
{
    public PlayerMovement playerMovement;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider that entered the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Check the name of the cloud
            string cloudName = gameObject.name;

            // Move the player to different destinations based on the cloud's name
            switch (cloudName)
            {
                case "Cloud1":
                    bool doubleJump = PlayerMovement.canDoubleJump;
                    if (doubleJump)
                    {
                        other.transform.position = new Vector3(0f, 110f, other.transform.position.z);
                    }
                    break;

                case "Cloud2":
                    other.transform.position = new Vector3(150f, 100f, other.transform.position.z);
                    break;

                // Add more cases for other cloud names as needed

                default:
                    // Move to a default position if the cloud name doesn't match any case
                    other.transform.position = new Vector3(0f, 0f, other.transform.position.z);
                    break;
            }
        }
    }
}
