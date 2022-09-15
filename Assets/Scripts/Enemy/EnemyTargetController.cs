using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyTargetType
{
    EnableEnemyTarget,
    DisableEnemyTarget
}

public class EnemyTargetController : MonoBehaviour
{
    [SerializeField]
    private EnemyTargetType enemyTargetType;

    [SerializeField]
    private EnemyBatchHandler enemyBatch;

    /*
    [SerializeField]
    private BossMovement bossEnemy;
    */
    [SerializeField]
    private bool bossZoneDetection;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (bossZoneDetection)
        {
            /*
            if (collision.CompareTag(TagManager.PLAYER_TAG))
            {

                if (enemyTargetType == EnemyTargetType.EnableEnemyTarget
                    && bossEnemy)
                {
                    bossEnemy.PlayerDetectedInfo(true);
                }
                else if (enemyTargetType == EnemyTargetType.DisableEnemyTarget
                    && bossEnemy)
                {
                    bossEnemy.PlayerDetectedInfo(false);
                }

            }
            */
        }
        else
        {

            if (collision.CompareTag(TagManager.PLAYER_TAG))
            {
                if (enemyTargetType == EnemyTargetType.EnableEnemyTarget)
                    enemyBatch.EnablePlayerTarget();
                else
                    enemyBatch.DisablePlayerTarget();
            }

        }

    }

}
