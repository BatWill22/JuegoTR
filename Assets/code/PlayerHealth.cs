using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 7;
    public int currentHealth;
    public Vector2 respawnCoordinates = new Vector2(0, 1);
    public Vector2 checkpointCoordinates = new Vector2(-81, 29);
    private bool invincible = false;
    public float invincibilityDuration = 1.5f;

    public bool redDoorOpen = false;
    public bool greenDoorOpen = false;
    public bool blueDoorOpen = false;

    // public DashItemScript dashItemScript;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        // dashItemScript = GetComponent<DashItemScript>();
    }

    // Public function to decrease player's health
    public void TakeDamage(int damageAmount)
    {
        if (!invincible)
        {
            currentHealth -= damageAmount;
            // Check if the player's health is below or equal to zero
            if (currentHealth <= 0)
            {
                // Player is defeated, respawn the player
                Respawn(); //hacer que si tienes las tres llaves y las tres habilidades reaparezcas en la zona final en vez de morir, a modo de unico checkpoint
            }
            else
            {
                StartCoroutine(StartInvincibility());
                Debug.Log("HEALTH POINTS: " + currentHealth);
            }
        }
    }

    // Public function to increase player's health
    // public void Heal(int healAmount)
    // {
    //     currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);

    //     Debug.Log("Player healed. Current health: " + currentHealth);
    // }

    // Respawn the player at specified coordinates
    private void Respawn() 
    {
        // Move the player to the respawn coordinates
        // if (playerM != null)
        // {
            // Debug.Log("Detecta playerMovement");
            // Debug.Log("Paso 3: se Llama a la función Respawn");
            bool dash = PlayerMovement.canDash;
            bool walljump = PlayerMovement.canWallJumpAndSlide;
            bool doubleJump = PlayerMovement.canDoubleJump;
            bool redKey = PlayerMovement.hasRedKey;
            bool greenKey = PlayerMovement.hasGreenKey;
            bool blueKey = PlayerMovement.hasBlueKey;
            // if(gameOver)
            // {
            //     Restart();
            //     Debug.Log("CONGRATULATIONS: YOU HAVE SUCCESFULLY COMPLETED THE GAME");
            // }
            if(dash && walljump && doubleJump && redKey && greenKey && blueKey && greenDoorOpen && blueDoorOpen && redDoorOpen)
            {
                transform.position = new Vector3(checkpointCoordinates.x, checkpointCoordinates.y, transform.position.z);
                // Debug.Log("tp a checkpoint");
            }
            else
            {
                Restart();
                SceneManager.LoadScene("DeathMenu");
            }
        // }
        // else
        // {
        //     transform.position = new Vector3(respawnCoordinates.x, respawnCoordinates.y, transform.position.z);
        // }

        // Reset the player's health to maximum
        currentHealth = maxHealth;

        // Debug.Log("Player respawned at coordinates: " + respawnCoordinates);
    }

    public void EnemyInteraction(Vector2 pushDirection, float pushForce)
    {
        // Call KnockBack from PlayerMovement script
        GetComponent<PlayerMovement>().KnockBack(pushDirection, pushForce);

        // Call TakeDamage from this script
        TakeDamage(1); // Assuming the enemy deals 1 damage, adjust as needed
    }

    private IEnumerator StartInvincibility()
    {
        invincible = true;

        // Wait for the specified duration
        yield return new WaitForSeconds(invincibilityDuration);

        // End of invincibility
        invincible = false;
    }

    public void ActivateRedDoor(bool activate)
    {
        redDoorOpen = activate;
    }
    public void ActivateGreenDoor(bool activate)
    {
        greenDoorOpen = activate;
    }
    public void ActivateBlueDoor(bool activate)
    {
        blueDoorOpen = activate;
    }

    // public void ActivateFinal(bool activate)
    // {
    //     gameOver = activate;
    //     Debug.Log("CONGRATULATIONS: YOU HAVE SUCCESFULLY COMPLETED THE GAME");
    //     // Debug.Log("Paso 2: se activa la variable del Game Over");
    //     Restart();
    // }

    private void Restart() 
    {
        // Debug.Log("Paso 5: se Llama a la función Restart");
        transform.position = new Vector3(respawnCoordinates.x, respawnCoordinates.y, transform.position.z);
        // if (dashItemScript != null)
        // {
        // Call the Respawn method from DashItemScript
        DashItemScript dashItemScript = FindObjectOfType<DashItemScript>();
        if (dashItemScript != null)
        {
            // Debug.Log("DEVUELVE ITEM DAS");
            dashItemScript.Respawn();
        }
        DoubleJumpItemScript doubleJumpItemScript = FindObjectOfType<DoubleJumpItemScript>();
        if (doubleJumpItemScript != null)
        {
            // Debug.Log("DEVUELVE ITEM DUBEJUM");
            doubleJumpItemScript.Respawn();
        }
        WalljumpItemScript walljumpItemScript = FindObjectOfType<WalljumpItemScript>();
        if (walljumpItemScript != null)
        {
            // Debug.Log("DEVUELVE ITEM WALJUM");
            walljumpItemScript.Respawn();
        }

        // FinalItemScript finalItemScript = FindObjectOfType<FinalItemScript>();
        // if (finalItemScript != null)
        // {
        //     // Debug.Log("DEVUELVE ITEM WALJUM");
        //     finalItemScript.Respawn();
        // }

        RedKeyItemScript redKeyItemScript = FindObjectOfType<RedKeyItemScript>();
        if (redKeyItemScript != null)
        {
            // Debug.Log("DEVUELVE ITEM RED");
            redKeyItemScript.Respawn();
        }
        GreenKeyItemScript greenKeyItemScript = FindObjectOfType<GreenKeyItemScript>();
        if (greenKeyItemScript != null)
        {
            // Debug.Log("DEVUELVE ITEM GRIN");
            greenKeyItemScript.Respawn();
        }
        BlueKeyItemScript blueKeyItemScript = FindObjectOfType<BlueKeyItemScript>();
        if (blueKeyItemScript != null)
        {
            // Debug.Log("DEVUELVE ITEM BLU");
            blueKeyItemScript.Respawn();
        }

        RedDoorObjectScript redDoorObjectScript = FindObjectOfType<RedDoorObjectScript>();
        if (redDoorObjectScript != null)
        {
            // Debug.Log("DEVUELVE ITEM WALJUM");
            redDoorObjectScript.Respawn();
        }
        GreenDoorObjectScript greenDoorObjectScript = FindObjectOfType<GreenDoorObjectScript>();
        if (greenDoorObjectScript != null)
        {
            // Debug.Log("DEVUELVE ITEM WALJUM");
            greenDoorObjectScript.Respawn();
        }
        BlueDoorObjectScript blueDoorObjectScript = FindObjectOfType<BlueDoorObjectScript>();
        if (blueDoorObjectScript != null)
        {
            // Debug.Log("DEVUELVE ITEM WALJUM");
            blueDoorObjectScript.Respawn();
        }
        FinalDoorObjectScript finalDoorObjectScript = FindObjectOfType<FinalDoorObjectScript>();
        if (finalDoorObjectScript != null)
        {
            // Debug.Log("DEVUELVE ITEM WALJUM");
            finalDoorObjectScript.Respawn();
        }
        ExitDoorObjectScript exitDoorObjectScript = FindObjectOfType<ExitDoorObjectScript>();
        if (exitDoorObjectScript != null)
        {
            // Debug.Log("DEVUELVE ITEM WALJUM");
            exitDoorObjectScript.Respawn();
        }
        NormalDoorObjectScript normalDoorObjectScript = FindObjectOfType<NormalDoorObjectScript>();
        if (normalDoorObjectScript != null)
        {
            // Debug.Log("DEVUELVE ITEM WALJUM");
            normalDoorObjectScript.Respawn();
        }

        // EnemyHealth enemyHealth = FindObjectOfType<EnemyHealth>();
        // if (enemyHealth != null)
        // {
        //     Debug.Log("DEVUELVE ITEM WALJUM");
        //     enemyHealth.Respawn();
        // }

        EnemyHealth[] enemies = FindObjectsOfType<EnemyHealth>();

        foreach (EnemyHealth enemy in enemies)
        {
            // Now you can call functions or perform actions on each enemy
            enemy.Respawn(); // Replace YourFunction() with the actual function you want to call
        }

        redDoorOpen = false;
        greenDoorOpen = false;
        blueDoorOpen = false;
        // Debug.Log("DEVUELVE ITEMS");
        // }
        // Debug.Log("tp a 0,0");
        // Debug.Log("Paso 7: todos los objetos vuelven a su posición");
    }
}
