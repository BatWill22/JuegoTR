using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public ActivateAndDeactivatePauseMenu activateAndDeactivatePauseMenu;

    public void StartGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Continue()
    {
        if (activateAndDeactivatePauseMenu != null)
        {
            activateAndDeactivatePauseMenu.Disappear();
        }
        // GameObject[] items = GameObject.FindGameObjectsWithTag("PauseElement");

        // foreach (GameObject item in items)
        // {
        //     item.transform.position = new Vector3(-200f, -20f, 0f);
        // }
    }

    public void Respawn()
    {
        playerMovement.transform.position = new Vector3(0f, 1f, 0f);
        Continue();
    }
}
