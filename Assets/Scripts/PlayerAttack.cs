using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 2.0f;
    public LayerMask enemyLayer;
    public float attackDamage = 10f;
    private Animator animator;

    private InputAction inputAction;

    private void Awake()
    {
        inputAction = new InputAction("Attack", binding: "<Mouse>/leftButton");
    }

    private void Start()
    {
        GameObject childObject = GameObject.FindGameObjectWithTag("PlayerAnimation");
        if (childObject != null)
        {
            animator = childObject.GetComponent<Animator>();
        }
        else
        {
            Debug.LogError("Child object with Animator not found. Make sure the tag is set correctly.");
        }
    }

    private void OnEnable()
    {
        inputAction.performed += ctx => StartAttack();
        inputAction.Enable();
    }

    private void OnDisable()
    {
        inputAction.performed -= ctx => StartAttack();
        inputAction.Disable();
    }

    public void StartAttack()
    {
        // Set a trigger instead of a bool
        animator.SetTrigger("StartAttackingKnife");
        Debug.Log("attacking");
    }


    public void Attack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);
        foreach (Collider enemyCollider in hitEnemies)
        {
            if (enemyCollider.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.TakeDamage(attackDamage);
                Debug.Log("Hitting" + " " + enemy.name + " " + "for " + " " + attackDamage + " " + "damage.");
                Debug.Log("Enemy Health: " + enemy.currentHealth);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (transform == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
