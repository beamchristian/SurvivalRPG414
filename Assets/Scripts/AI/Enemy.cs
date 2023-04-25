using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;


    public float CurrentHealth { get; private set; }

    private Animator animator;
    public bool IsDead { get; private set; }
    private EnemyAI enemyAI;
    public NeedsSystem playerNeedsSystem; // Add this line

    private void Start()
    {
        CurrentHealth = maxHealth;
        animator = GetComponentInChildren<Animator>();
        enemyAI = GetComponent<EnemyAI>(); // Get the EnemyAI component


        GameObject player = GameObject.FindWithTag("Player"); // Add this line
        if (player != null) // Add this line
        {
            playerNeedsSystem = player.GetComponent<NeedsSystem>(); // Add this line
        }
    }

    private void PlayGotHitAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("GotHit");
        }
    }


    public void TakeDamage(float damage, Vector3 attackerPosition)
    {
        if (IsDead) return;

        CurrentHealth -= damage;
        PlayGotHitAnimation(); // Add this line

        enemyAI.OnPlayerAttack();


        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        IsDead = true;
        animator.SetTrigger("Death");
        StartCoroutine(DestroyAfterDelay(5f));
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
