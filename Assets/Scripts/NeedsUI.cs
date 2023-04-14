using UnityEngine;
using UnityEngine.UI;

public class NeedsUI : MonoBehaviour
{
    [Header("Needs System")]
    [SerializeField] private NeedsSystem needsSystem;

    [Header("UI Elements")]
    [SerializeField] private Image hungerBarFill;
    [SerializeField] private Image thirstBarFill;
    [SerializeField] private Image fatigueBarFill;
    [SerializeField] private Image healthBarFill;

    private void Update()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        float hunger = needsSystem.GetHunger();
        float thirst = needsSystem.GetThirst();
        float fatigue = needsSystem.GetFatigue();
        float health = needsSystem.GetCurrentHealth();
        float maxHealth = needsSystem.GetMaxHealth();

        hungerBarFill.fillAmount = hunger / 100f;
        thirstBarFill.fillAmount = thirst / 100f;
        fatigueBarFill.fillAmount = fatigue / 100f;
        healthBarFill.fillAmount = health / maxHealth;
    }
}
