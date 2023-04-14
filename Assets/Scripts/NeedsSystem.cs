using UnityEngine;

public class NeedsSystem : MonoBehaviour
{
    [Header("Needs")]
    [SerializeField] private float hunger = 100f;
    [SerializeField] private float thirst = 100f;
    [SerializeField] private float fatigue = 100f;

    [Header("Decay Rates")]
    [SerializeField] private float hungerDecayRate = 1f;
    [SerializeField] private float thirstDecayRate = 1f;
    [SerializeField] private float fatigueDecayRate = 1f;

    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        DecreaseNeeds(Time.deltaTime);
        UpdateHealth();
    }

    private void DecreaseNeeds(float deltaTime)
    {
        hunger -= hungerDecayRate * deltaTime;
        thirst -= thirstDecayRate * deltaTime;
        fatigue -= fatigueDecayRate * deltaTime;

        hunger = Mathf.Clamp(hunger, 0f, 100f);
        thirst = Mathf.Clamp(thirst, 0f, 100f);
        fatigue = Mathf.Clamp(fatigue, 0f, 100f);
    }

    private void UpdateHealth()
    {
        float healthDecrease = 0f;

        if (hunger <= 0) healthDecrease++;
        if (thirst <= 0) healthDecrease++;
        if (fatigue <= 0) healthDecrease++;

        currentHealth -= healthDecrease * Time.deltaTime;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    }

    public float GetHunger() { return hunger; }
    public float GetThirst() { return thirst; }
    public float GetFatigue() { return fatigue; }
    public float GetCurrentHealth() { return currentHealth; }
    public float GetMaxHealth() { return maxHealth; }
}
