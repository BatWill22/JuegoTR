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
    private EnemyMovement enemyMovement;
    [SerializeField] private Animator animator;

    public float countdownDuration = 0.1f; // Set the duration of the countdown in seconds
    private float timer;

    [Header("Sounds")]
    [SerializeField] private AudioSource enemyHit;

    // Start is called before the first frame update
    void Start()
    {
        currentEnemyHealth = maxEnemyHealth;
        initialCoordinates = transform.position;
        hiddenCoordinates = new Vector3(-200, -30, 0);
        enemy = gameObject;
        enemyCollider = GetComponent<Collider2D>();
        timer = 2;
        enemyMovement = GetComponent<EnemyMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rightRaycastOrigin = transform.position + new Vector3(enemyCollider.bounds.extents.x, 0f, 0f);
        Vector3 leftRaycastOrigin = transform.position - new Vector3(enemyCollider.bounds.extents.x, 0f, 0f);
        Vector3 centerRaycastOrigin = transform.position;
        // Perform raycasts
        bool rightRaycastHit = Physics2D.Raycast(rightRaycastOrigin, Vector2.down, enemyCollider.bounds.extents.y*1.5f, obstacleLayer);
        bool leftRaycastHit = Physics2D.Raycast(leftRaycastOrigin, Vector2.down, enemyCollider.bounds.extents.y*1.5f, obstacleLayer);
        bool centerRaycastHit = Physics2D.Raycast(centerRaycastOrigin, Vector2.down, enemyCollider.bounds.extents.y*1.5f, obstacleLayer);

        Debug.DrawRay(rightRaycastOrigin, Vector2.down * (enemyCollider.bounds.extents.y*1.5f), (enemyCollider.IsTouchingLayers(obstacleLayer) || rightRaycastHit) ? Color.green : Color.red);
        Debug.DrawRay(leftRaycastOrigin, Vector2.down * (enemyCollider.bounds.extents.y*1.5f), (enemyCollider.IsTouchingLayers(obstacleLayer) || leftRaycastHit) ? Color.green : Color.red);
        Debug.DrawRay(centerRaycastOrigin, Vector2.down * (enemyCollider.bounds.extents.y*1.5f), (enemyCollider.IsTouchingLayers(obstacleLayer) || centerRaycastHit) ? Color.green : Color.red);

        if ((rightRaycastHit || leftRaycastHit || centerRaycastHit) && !(timer <= countdownDuration))
        {
            Debug.Log("entra en el MUERTEEEE del object");
            animator.SetTrigger ("Hide");
            Die2();
        }

        if (timer <= countdownDuration)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                Die();
            }
        }
    }

    private void Die2()
    {
        timer = countdownDuration;
        enemyMovement.EnemyMove(false);
    }

    public void GetHit()
    {
        enemyHit.Play();
        currentEnemyHealth--;
        if(currentEnemyHealth <= 0)
        {
            animator.SetTrigger ("Hide");
            Die2();
        }
        else
        {
            animator.SetTrigger ("Hitted");
        }
    }

    public void Die()
    {
        StopAllCoroutines();
        Destroy(gameObject);
        // transform.position = hiddenCoordinates;
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
            timer = 15;
            enemyMovement.EnemyMove(true);
            // if (playerHealth != null)
            // {
            //     // Call ActivateDoubleJump function from PlayerHealth script
            //     playerHealth.ActivateBlueDoor(false);
            // }
            // Debug.Log("BlueDoor object respawned! jbaeuoebngtoqbhaweughoiaoghuaheihjyopashia0opeghuohgpaiheoughoanepighaouge");
        }
    }
}
