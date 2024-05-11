using JustGame.Scripts.Enemy;
using UnityEngine;

public class EnemyMoveToRandomDirection : BrainAction
{
    public override void DoAction()
    {
        var randomDir = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        var enemyMovement = m_brain.EnemyController.GetComponent<EnemyMovement>();
        if (enemyMovement == null) return;
        enemyMovement.SetDirection(randomDir);
    }
}
