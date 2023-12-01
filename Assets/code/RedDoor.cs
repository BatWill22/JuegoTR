using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedDoorObjectScript : MonoBehaviour
{
    private Vector3 initialRedDoorObjectPosition;

    public GameObject redDoorObject;

    public PlayerHealth playerHealth;

    private Vector3 hiddenRedDoorObjectPosition;

    private void Start()
    {
        initialRedDoorObjectPosition = transform.position;
        hiddenRedDoorObjectPosition = new Vector3(-200, -20, 0);
    }

    public void OpenDoor()
    {
        redDoorObject.transform.position = hiddenRedDoorObjectPosition;
        if (playerHealth != null)
        {
            playerHealth.ActivateRedDoor(true);
        }
    }

    public void Respawn()
    {
        // Debug.Log("entra en el respawn del object");
        // Respawn the RedDoor object if a reference exists
        if (redDoorObject != null)
        {
            redDoorObject.SetActive(true);
            // You might want to set its position to the initial spawn position
            redDoorObject.transform.position = initialRedDoorObjectPosition;
            if (playerHealth != null)
            {
                // Call ActivateDoubleJump function from PlayerHealth script
                playerHealth.ActivateRedDoor(false);
            }
            // Debug.Log("RedDoor object respawned! jbaeuoebngtoqbhaweughoiaoghuaheihjyopashia0opeghuohgpaiheoughoanepighaouge");
        }
    }
}
