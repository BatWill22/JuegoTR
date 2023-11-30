using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenDoorObjectScript : MonoBehaviour
{
    private Vector3 initialGreenDoorObjectPosition;

    public GameObject greenDoorObject;

    public PlayerHealth playerHealth;

    private Vector3 hiddenGreenDoorObjectPosition;

    private void Start()
    {
        initialGreenDoorObjectPosition = transform.position;
        hiddenGreenDoorObjectPosition = new Vector3(-200, -20, 0);
    }

    public void OpenDoor()
    {
        greenDoorObject.transform.position = hiddenGreenDoorObjectPosition;
        if (playerHealth != null)
        {
            playerHealth.ActivateGreenDoor(true);
        }
    }

    public void Respawn()
    {
        Debug.Log("entra en el respawn del object");
        // Respawn the GreenDoor object if a reference exists
        if (greenDoorObject != null)
        {
            greenDoorObject.SetActive(true);
            // You might want to set its position to the initial spawn position
            greenDoorObject.transform.position = initialGreenDoorObjectPosition;
            if (playerHealth != null)
            {
                // Call ActivateDoubleJump function from PlayerHealth script
                playerHealth.ActivateGreenDoor(false);
            }
            Debug.Log("GreenDoor object respawned! jbaeuoebngtoqbhaweughoiaoghuaheihjyopashia0opeghuohgpaiheoughoanepighaouge");
        }
    }
}
