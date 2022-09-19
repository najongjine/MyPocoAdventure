using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBatchHandler : MonoBehaviour
{
    [SerializeField]
    private bool hasShooterEnemies;

    [SerializeField]
    private List<CharacterMovement> enemies;

    [SerializeField]
    private Transform shooterEnemyHolder;

    [SerializeField]
    private List<EnemyShooter> shooterEnemies;

    [SerializeField]
    private GameObject batchDoor;

    private void Start()
    {

        foreach (Transform tr in GetComponentInChildren<Transform>())
        {
            // make sure it's child transform
            if (tr != this)
                enemies.Add(tr.GetComponent<CharacterMovement>());
        }

        if (hasShooterEnemies)
        {
            foreach (Transform tr in shooterEnemyHolder.GetComponentInChildren<Transform>())
            {
                shooterEnemies.Add(tr.GetComponent<EnemyShooter>());
            }
        }

    }

    public void EnablePlayerTarget()
    {
        foreach (CharacterMovement charMovement in enemies)
            charMovement.HasPlayerTarget = true;
    }

    public void DisablePlayerTarget()
    {
        foreach (CharacterMovement charMovement in enemies)
            charMovement.HasPlayerTarget = false;
    }

    public void RemoveEnemy(CharacterMovement enemy)
    {
        enemies.Remove(enemy);

        CheckToUnlockDoor();

    }
    public void RemoveShooterEnemy(EnemyShooter shooterEnemy)
    {

        if (shooterEnemies != null)
            shooterEnemies.Remove(shooterEnemy);

        CheckToUnlockDoor();

    }
    void CheckToUnlockDoor()
    {
        if (hasShooterEnemies)
        {
            if (enemies.Count == 0 && shooterEnemies.Count == 0)
            {
                if (batchDoor)
                    batchDoor.SetActive(false);
            }
        }
        else
        {
            if (enemies.Count == 0)
            {
                if (batchDoor)
                    batchDoor.SetActive(false);
            }
        }
    }

}
