using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackRange = 2.0f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 0.5f;

    private Animator animator;
    private float nextAttackTime;
    private InputAction inputAction;
    private Transform cachedTransform;

    private void Awake()
    {
        inputAction = new InputAction("Attack", binding: "<Mouse>/leftButton");
        cachedTransform = transform;
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
        if (Time.time >= nextAttackTime)
        {
            // Set a trigger instead of a bool
            animator.SetTrigger("StartAttackingKnife");
            Debug.Log("attacking");

            // Update the nextAttackTime
            nextAttackTime = Time.time + attackCooldown;
        }
    }


    public void Attack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);
        foreach (Collider enemyCollider in hitEnemies)
        {
            if (enemyCollider.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.TakeDamage(attackDamage, cachedTransform.position); // Modify this line
                Debug.Log("Hitting" + " " + enemy.name + " " + "for " + " " + attackDamage + " " + "damage.");
                Debug.Log("Enemy Health: " + enemy.CurrentHealth);
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
