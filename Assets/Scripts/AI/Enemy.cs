using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    private Animator animator;
    public bool isDead = false;

    private EnemyAI enemyAI; // Add a reference to the EnemyAI component

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponentInChildren<Animator>();
        enemyAI = GetComponent<EnemyAI>(); // Get the EnemyAI component
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        enemyAI.OnPlayerAttack(); // Call the OnPlayerAttack method when the NPC takes damage

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        animator.SetTrigger("Death");
        StartCoroutine(DestroyAfterDelay(5f));
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
