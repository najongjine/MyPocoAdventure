using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [SerializeField]
    private float normalMovementSpeed = 0.5f, playerDetectedMovementSpeed = 1f;

    private float moveSpeed;

    [SerializeField]
    private Transform[] movementPositions;

    private Vector3 targetPosition;

    private Vector3 myScale;

    private bool playerDetected;

    private Transform playerTarget;

    private bool chasePlayer;

    [SerializeField]
    private float damageAmount = 10f;

    [SerializeField]
    private float shootTimeDelay = 2f;
    private float shootTimer;

    private EnemyShootController enemyShootController;

    private CharacterHealth bossHealth;

    [SerializeField]
    private GameObject door;

    private void Start()
    {
        moveSpeed = normalMovementSpeed;
        GetRandomMovementPosition();

        playerTarget = GameObject.FindWithTag(TagManager.PLAYER_TAG).transform;

        enemyShootController = GetComponent<EnemyShootController>();
        bossHealth = GetComponent<CharacterHealth>();

    }

    private void OnDisable()
    {
        if (!bossHealth.IsAlive())
            door.SetActive(false);
    }

    private void Update()
    {
        if (!playerTarget)
            return;

        if (!bossHealth.IsAlive())
            return;

        HandleMovement();
        HandleFacingDirection();

        HandleShooting();

    }

    void GetRandomMovementPosition()
    {
        int randomIndex = Random.Range(0, movementPositions.Length);

        while (targetPosition == movementPositions[randomIndex].position)
        {
            randomIndex = Random.Range(0, movementPositions.Length);
        }

        targetPosition = movementPositions[randomIndex].position;
    }

    void HandleMovement()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition,
            moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {

            if (playerDetected)
            {
                if (Random.Range(0, 10) > 7)
                {
                    // research player loc. if not, go to old player loc
                    targetPosition = playerTarget.position;
                    chasePlayer = true;
                }
            }
            else
            {
                if (!chasePlayer)
                {
                    GetRandomMovementPosition();
                }
            }

        }

    }

    void HandleFacingDirection()
    {
        myScale = transform.localScale;

        if (targetPosition.x > transform.position.x)
            myScale.x = Mathf.Abs(myScale.x);
        else if (targetPosition.x < transform.position.x)
            myScale.x = -Mathf.Abs(myScale.x);

        transform.localScale = myScale;
    }

    void PlayerDetectedChangeMovementSpeed(bool detected)
    {
        if (detected)
            moveSpeed = playerDetectedMovementSpeed;
        else
            moveSpeed = normalMovementSpeed;
    }

    public void PlayerDetectedInfo(bool detected)
    {
        playerDetected = detected;
        PlayerDetectedChangeMovementSpeed(detected);

        if (!playerDetected)
        {
            chasePlayer = false;
            GetRandomMovementPosition();
        }
    }

    void HandleShooting()
    {
        
        if (playerDetected)
        {
            if (Time.time > shootTimer)
            {
                shootTimer = Time.time + shootTimeDelay;
                Vector2 direction = (playerTarget.position - transform.position).normalized;
                enemyShootController.ShootOnRandom(direction, transform.position);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagManager.PLAYER_TAG))
        {
            chasePlayer = false;
            GetRandomMovementPosition();

            collision.GetComponent<CharacterHealth>().TakeDamage(damageAmount);
        }
    }

}
