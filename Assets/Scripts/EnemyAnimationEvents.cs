using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    private EnemyAI enemyAI;

    private void Start()
    {
        enemyAI = GetComponentInParent<EnemyAI>();
        if (enemyAI == null)
        {
            Debug.LogError("PlayerAttack script not found on the parent object.");
        }
    }

    public void TriggerPlayerDamage()
    {
        if (enemyAI != null)
        {
            enemyAI.ApplyDamageToPlayer();
        }
    }
}
