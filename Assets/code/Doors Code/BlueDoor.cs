using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueDoorObjectScript : MonoBehaviour
{
    private Vector3 initialBlueDoorObjectPosition;

    public GameObject blueDoorObject;

    public PlayerHealth playerHealth;

    private Vector3 hiddenBlueDoorObjectPosition;

    private void Start()
    {
        initialBlueDoorObjectPosition = transform.position;
        hiddenBlueDoorObjectPosition = new Vector3(-200, -20, 0);
    }

    public void OpenDoor()
    {
        blueDoorObject.transform.position = hiddenBlueDoorObjectPosition;
        if (playerHealth != null)
        {
            playerHealth.ActivateBlueDoor(true);
        }
    }

    public void Respawn()
    {
        Debug.Log("entra en el respawn del object");
        // Respawn the BlueDoor object if a reference exists
        if (blueDoorObject != null)
        {
            blueDoorObject.SetActive(true);
            // You might want to set its position to the initial spawn position
            blueDoorObject.transform.position = initialBlueDoorObjectPosition;
            if (playerHealth != null)
            {
                // Call ActivateDoubleJump function from PlayerHealth script
                playerHealth.ActivateBlueDoor(false);
            }
            Debug.Log("BlueDoor object respawned! jbaeuoebngtoqbhaweughoiaoghuaheihjyopashia0opeghuohgpaiheoughoanepighaouge");
        }
    }
}
