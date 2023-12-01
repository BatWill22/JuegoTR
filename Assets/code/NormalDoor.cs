using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalDoorObjectScript : MonoBehaviour
{
    private Vector3 initialNormalDoorObjectPosition;

    public GameObject normalDoorObject;

    public PlayerHealth playerHealth;

    private Vector3 hiddenNormalDoorObjectPosition;

    private void Start()
    {
        initialNormalDoorObjectPosition = transform.position;
        hiddenNormalDoorObjectPosition = new Vector3(-200, -20, 0);
        // normalDoorObject = GetComponent<GameObject>();
    }

    public void OpenDoor()
    {
        normalDoorObject.transform.position = hiddenNormalDoorObjectPosition;
    }

    public void Respawn()
    {
        // Respawn the NormalDoor object if a reference exists
        if (normalDoorObject != null)
        {
            // You might want to set its position to the initial spawn position
            normalDoorObject.transform.position = initialNormalDoorObjectPosition;
            // Debug.Log("NormalDoor object respawned! jbaeuoebngtoqbhaweughoiaoghuaheihjyopashia0opeghuohgpaiheoughoanepighaouge");
        }
    }
}
