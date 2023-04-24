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

    private void PlayGotHitAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("GotHit");
        }
    }


    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        PlayGotHitAnimation(); // Add this line

        enemyAI.OnPlayerAttack();

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
