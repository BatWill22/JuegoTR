using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalTriggerZoneScript : MonoBehaviour
{
    public GameObject finalTriggerZone;

    public PlayerHealth playerHealth;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().ActivateFinalTriggerZone(true);
            Debug.Log("entra");
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().ActivateFinalTriggerZone(false);
            Debug.Log("se va");
        }
    }
}
