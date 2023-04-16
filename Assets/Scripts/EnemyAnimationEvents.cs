using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    private EnemyAI enemyAI;

    private void Start()
    {
        enemyAI = GetComponentInParent<EnemyAI>();
    }

    public void OnAttackFinished()
    {
        if (enemyAI != null)
        {
            enemyAI.OnAttackFinished();
        }
    }
}
