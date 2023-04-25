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
    private float currentPlayerHealth;

    public GameObject gameOverPanel;

    private float gameOverDelay = 3f;
    private float gameOverTimer;
    private bool isGameOver = false;
    private Vector3 knockbackDirection;


    private void Start()
    {
        currentPlayerHealth = maxHealth;
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
        return currentPlayerHealth <= 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            HandleDeath();
        }
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

    public void AddHealth(float value)
    {
        currentPlayerHealth = Mathf.Clamp(currentPlayerHealth + value, 0f, maxHealth);
    }

    public void AddHunger(float value)
    {
        hunger = Mathf.Clamp(hunger + value, 0f, 100f);
    }

    public void AddThirst(float value)
    {
        thirst = Mathf.Clamp(thirst + value, 0f, 100f);
    }

    public void AddFatigue(float value)
    {
        fatigue += value;
        fatigue = Mathf.Clamp(fatigue, 0f, 100f);
    }

    public void ApplyDamage(float damage)
    {
        currentPlayerHealth -= damage;
        currentPlayerHealth = Mathf.Clamp(currentPlayerHealth, 0f, maxHealth);
    }

    void HandleDeath()
    {
        isGameOver = true;

        // Show Game Over panel
        gameOverPanel.SetActive(true);

        // Set the timer to restart the scene after the delay
        gameOverTimer = gameOverDelay;
    }

/*    void PauseGame ()
    {
        Time.timeScale = 0;
    }
    void ResumeGame ()
    {
        Time.timeScale = 1;
    }*/

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void KnockbackPlayer(Vector3 attackerPosition, float knockbackForce)
    {
        Debug.Log("Knockback Called");
        // Assuming you have a CharacterController component on your player
        CharacterController characterController = GetComponent<CharacterController>();
        Vector3 knockbackDirection = (transform.position - attackerPosition).normalized;
        Vector3 knockback = knockbackDirection * knockbackForce;

        // Apply the knockback force to the player's character controller
       characterController.SimpleMove(knockback);
    }


    private void UpdateHealth()
    {
        float healthDecrease = 0f;

        if (hunger <= 0) healthDecrease++;
        if (thirst <= 0) healthDecrease++;
        if (fatigue <= 0) healthDecrease++;

        currentPlayerHealth -= healthDecrease * Time.deltaTime;
        currentPlayerHealth = Mathf.Clamp(currentPlayerHealth, 0f, maxHealth);

        if (currentPlayerHealth <= 0 && !isGameOver)
        {
            HandleDeath();
        }
    }

    public float GetHunger() { return hunger; }
    public float GetThirst() { return thirst; }
    public float GetFatigue() { return fatigue; }
    public float GetCurrentPlayerHealth() { return currentPlayerHealth; }
    public float GetMaxHealth() { return maxHealth; }
}
