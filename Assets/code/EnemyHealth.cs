using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxEnemyHealth = 3;
    public int currentEnemyHealth;
    private Collider2D enemyCollider;
    private GameObject enemy;
    public LayerMask obstacleLayer;
    public Vector2 initialCoordinates;
    private Vector3 hiddenCoordinates;

    // Start is called before the first frame update
    void Start()
    {
        currentEnemyHealth = maxEnemyHealth;
        initialCoordinates = transform.position;
        hiddenCoordinates = new Vector3(-200, -30, 0);
        enemy = gameObject;
        enemyCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rightRaycastOrigin = transform.position + new Vector3(enemyCollider.bounds.extents.x, 0f, 0f);
        Vector3 leftRaycastOrigin = transform.position - new Vector3(enemyCollider.bounds.extents.x, 0f, 0f);
        Vector3 centerRaycastOrigin = transform.position;
        // Perform raycasts
        bool rightRaycastHit = Physics2D.Raycast(rightRaycastOrigin, Vector2.down, enemyCollider.bounds.extents.y + 0.01f, obstacleLayer);
        bool leftRaycastHit = Physics2D.Raycast(leftRaycastOrigin, Vector2.down, enemyCollider.bounds.extents.y + 0.01f, obstacleLayer);
        bool centerRaycastHit = Physics2D.Raycast(centerRaycastOrigin, Vector2.down, enemyCollider.bounds.extents.y + 0.01f, obstacleLayer);

        if (rightRaycastHit || leftRaycastHit || centerRaycastHit)
        {
            Die();
        }
    }

    public void GetHit()
    {
        currentEnemyHealth--;
        if(currentEnemyHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        transform.position = hiddenCoordinates;
    }

    public void Respawn()
    {
        // Debug.Log("entra en el respawn del object");
        // Respawn the BlueDoor object if a reference exists
        if (enemy != null)
        {
            // blueDoorObject.SetActive(true);
            // You might want to set its position to the initial spawn position
            enemy.transform.position = initialCoordinates;
            currentEnemyHealth = maxEnemyHealth;
            // if (playerHealth != null)
            // {
            //     // Call ActivateDoubleJump function from PlayerHealth script
            //     playerHealth.ActivateBlueDoor(false);
            // }
            // Debug.Log("BlueDoor object respawned! jbaeuoebngtoqbhaweughoiaoghuaheihjyopashia0opeghuohgpaiheoughoanepighaouge");
        }
    }
}
