using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoorObjectScript : MonoBehaviour
{
    private Vector3 initialExitDoorObjectPosition;

    public GameObject exitDoorObject;

    public PlayerHealth playerHealth;

    private Vector3 hiddenExitDoorObjectPosition;

    private void Start()
    {
        initialExitDoorObjectPosition = transform.position;
        hiddenExitDoorObjectPosition = new Vector3(-200, -20, 0);
    }

    public void OpenDoor()
    {
        exitDoorObject.transform.position = hiddenExitDoorObjectPosition;
        // if (playerHealth != null)
        // {
        //     playerHealth.ActivateDoor(true);
        // }
    }

    public void Respawn()
    {
        // Debug.Log("entra en el respawn del object");
        // Respawn the Door object if a reference exists
        if (exitDoorObject != null)
        {
            // ExitDoorObject.SetActive(true);
            // You might want to set its position to the initial spawn position
            exitDoorObject.transform.position = initialExitDoorObjectPosition;
            // if (playerHealth != null)
            // {
            //     // Call ActivateDoubleJump function from PlayerHealth script
            //     playerHealth.ActivateDoor(false);
            // }
            // Debug.Log("Door object respawned! jbaeuoebngtoqbhaweughoiaoghuaheihjyopashia0opeghuohgpaiheoughoanepighaouge");
        }
    }
}
