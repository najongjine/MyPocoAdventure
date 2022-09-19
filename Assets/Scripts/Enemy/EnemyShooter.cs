using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyShooterType
{
    Horizontal,
    Vertical,
    Stationary
}
public class EnemyShooter : MonoBehaviour
{
    [SerializeField]
    private EnemyShooterType enemyType = EnemyShooterType.Horizontal;

    private float min_XY_Pos, max_XY_Pos;
    private Vector3 minPos, maxPos;

    [SerializeField]
    private float changingPosition_Delay = 2f;
    private float changingPosition_Timer;

    private Vector3 startingPosition;
    private Vector3 targetPosition;

    private bool changedPosition;

    [SerializeField]
    private float moveSpeed = 0.75f;

    private Vector3 myScale;

    private EnemyShootController enemyShootController;

    private bool playerInRange;

    [SerializeField]
    private float shootTimeDelay = 2f;
    private float shootTimer;

    private Transform playerTransform;

    [SerializeField]
    private Transform bulletSpawnPosition;

    private CharacterHealth enemyHealth;

    [SerializeField]
    private EnemyBatchHandler enemyBatch;

    private void Awake()
    {

        startingPosition = transform.position;

        if (enemyType == EnemyShooterType.Horizontal)
        {
            min_XY_Pos = transform.GetChild(0).transform.localPosition.x;
            max_XY_Pos = transform.GetChild(1).transform.localPosition.x;
        }
        else if (enemyType == EnemyShooterType.Vertical)
        {
            min_XY_Pos = transform.GetChild(0).transform.localPosition.y;
            max_XY_Pos = transform.GetChild(1).transform.localPosition.y;
        }
        else
        {
            minPos = transform.GetChild(0).transform.position;
            maxPos = transform.GetChild(1).transform.position;
            targetPosition = maxPos;
        }

        changingPosition_Timer = Time.time + changingPosition_Delay;

        enemyShootController = GetComponent<EnemyShootController>();

        enemyHealth = GetComponent<CharacterHealth>();

    }

    private void Start()
    {
        playerTransform = GameObject.FindWithTag(TagManager.PLAYER_TAG).transform;
    }

    private void OnDisable()
    {
        if (!enemyHealth.IsAlive())
            enemyBatch.RemoveShooterEnemy(this);
    }

    private void Update()
    {
        if (!enemyHealth.IsAlive())
            return;

        if (!playerTransform)
            return;

        CheckToShootPlayer();
    }

    private void FixedUpdate()
    {

        if (!enemyHealth.IsAlive())
            return;

        if (!playerTransform)
            return;

        EnemyMovement();
    }

    void EnemyMovement()
    {

        if (enemyType == EnemyShooterType.Horizontal)
        {
            if (!changedPosition)
            {
                float xPos = Random.Range(min_XY_Pos, max_XY_Pos);
                targetPosition = startingPosition + Vector3.right * xPos;
                changedPosition = true;
            }
        }
        else if (enemyType == EnemyShooterType.Vertical)
        {
            if (!changedPosition)
            {
                float yPos = Random.Range(min_XY_Pos, max_XY_Pos);
                targetPosition = startingPosition + Vector3.up * yPos;
                changedPosition = true;
            }
        }
        else
        {
            if (!changedPosition)
            {
                targetPosition = maxPos == targetPosition ? minPos : maxPos;
                changedPosition = true;
            }
        }

        if (Vector3.Distance(transform.position, targetPosition) <= 0.05f)
        {
            if (Time.time > changingPosition_Timer)
            {
                changedPosition = false;
                changingPosition_Timer = Time.time + changingPosition_Delay;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position,
            targetPosition, Time.deltaTime * moveSpeed);

        HandleFacingDirection();

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

    void CheckToShootPlayer()
    {
        if (playerInRange)
        {
            if (Time.time > shootTimer)
            {
                shootTimer = Time.time + shootTimeDelay;
                Vector2 direction = (playerTransform.position - bulletSpawnPosition.position).normalized;
                enemyShootController.Shoot(direction, bulletSpawnPosition.position);
            }
        }
    }

    public void SetPlayerInRange(bool inRange)
    {
        playerInRange = inRange;
    }

}
