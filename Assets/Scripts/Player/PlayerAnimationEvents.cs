using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private PlayerAttack playerAttack;

    private void Start()
    {
        playerAttack = GetComponentInParent<PlayerAttack>();
        if (playerAttack == null)
        {
            Debug.LogError("PlayerAttack script not found on the parent object.");
        }
    }

    public void TriggerDamage()
    {
        if (playerAttack != null)
        {
            playerAttack.Attack();
        }
    }
}
