using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

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

    public GameObject gameOverPanel;

    private float gameOverDelay = 3f;
    private float gameOverTimer;
    private bool isGameOver = false;


    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        DecreaseNeeds(Time.deltaTime);
        UpdateHealth();

        if (isGameOver)
        {
            gameOverTimer -= Time.deltaTime;
            if (gameOverTimer <= 0)
            {
                RestartScene();
            }
        }
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
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

    public void ApplyDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    }

    void HandleDeath()
    {
        isGameOver = true;

        // Show Game Over panel
        gameOverPanel.SetActive(true);

        // Set the timer to restart the scene after the delay
        gameOverTimer = gameOverDelay;
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void UpdateHealth()
    {
        float healthDecrease = 0f;

        if (hunger <= 0) healthDecrease++;
        if (thirst <= 0) healthDecrease++;
        if (fatigue <= 0) healthDecrease++;

        currentHealth -= healthDecrease * Time.deltaTime;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (currentHealth <= 0 && !isGameOver)
        {
            HandleDeath();
        }
    }

    public float GetHunger() { return hunger; }
    public float GetThirst() { return thirst; }
    public float GetFatigue() { return fatigue; }
    public float GetCurrentHealth() { return currentHealth; }
    public float GetMaxHealth() { return maxHealth; }
}
