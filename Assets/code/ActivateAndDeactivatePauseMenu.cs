using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAndDeactivatePauseMenu : MonoBehaviour
{
    public GameObject canvas;
    public PlayerMovement playerMovement;
    public PlayerAttack playerAttack;
    private bool isVisible = false;

    private void Start()
    {
        isVisible = false;
        Disappear();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isVisible = !isVisible;
            
            if (!isVisible)
            {
                canvas.SetActive(true);
                if (playerMovement != null)
                {
                    playerMovement.NotPaused(false);
                }
                if (playerAttack != null)
                {
                    playerAttack.CanAttack(false);
                }
                EnemyMovement[] enemyMovements = FindObjectsOfType<EnemyMovement>();
                // Call the desired function on each EnemyMovement script
                foreach (EnemyMovement enemyMovement in enemyMovements)
                {
                    enemyMovement.EnemyMove(false); // Replace YourFunctionName with the actual function name
                }
            }
            else
            {
                Disappear();
            }
        }
    }

    public void Disappear()
    {
        canvas.SetActive(false);
        if (playerMovement != null)
        {
            playerMovement.NotPaused(true);
        }
        if (playerAttack != null)
        {
            playerAttack.CanAttack(true);
        }
        EnemyMovement[] enemyMovements = FindObjectsOfType<EnemyMovement>();
        // Call the desired function on each EnemyMovement script
        foreach (EnemyMovement enemyMovement in enemyMovements)
        {
            enemyMovement.EnemyMove(true); // Replace YourFunctionName with the actual function name
        }
    }
}
