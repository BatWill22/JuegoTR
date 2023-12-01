using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoorObjectScript : MonoBehaviour
{
    private Vector3 initialFinalDoorObjectPosition;

    public GameObject finalDoorObject;

    public PlayerHealth playerHealth;

    private Vector3 hiddenFinalDoorObjectPosition;

    private void Start()
    {
        initialFinalDoorObjectPosition = transform.position;
        hiddenFinalDoorObjectPosition = new Vector3(-200, -20, 0);
    }

    public void OpenDoor()
    {
        finalDoorObject.transform.position = hiddenFinalDoorObjectPosition;
        // if (playerHealth != null)
        // {
        //     playerHealth.ActivateDoor(true);
        // }
    }

    public void Respawn()
    {
        // Debug.Log("entra en el respawn del object");
        // Respawn the Door object if a reference exists
        if (finalDoorObject != null)
        {
            // FinalDoorObject.SetActive(true);
            // You might want to set its position to the initial spawn position
            finalDoorObject.transform.position = initialFinalDoorObjectPosition;
            // if (playerHealth != null)
            // {
            //     // Call ActivateDoubleJump function from PlayerHealth script
            //     playerHealth.ActivateDoor(false);
            // }
            // Debug.Log("Door object respawned! jbaeuoebngtoqbhaweughoiaoghuaheihjyopashia0opeghuohgpaiheoughoanepighaouge");
        }
    }
}
