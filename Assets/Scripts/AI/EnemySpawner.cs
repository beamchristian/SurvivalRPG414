using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private int maxEnemies = 10;

    [Header("Spawn Settings")]
    [SerializeField] private Vector3 spawnAreaSize = new Vector3(50, 0, 50);
    [SerializeField] private float spawnDelay = 2f;
    [SerializeField] private float enemyActivationDelay = 0.5f;

    private float spawnTimer;

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnDelay && GameObject.FindGameObjectsWithTag("Enemy").Length < maxEnemies)
        {
            SpawnEnemy();
            spawnTimer = 0f;
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition = new Vector3(
            transform.position.x + Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            transform.position.y,
            transform.position.z + Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
        );

        int randomIndex = Random.Range(0, enemyPrefabs.Count);
        GameObject newEnemy = Instantiate(enemyPrefabs[randomIndex], spawnPosition, Quaternion.identity);
        newEnemy.tag = "Enemy";

        // Disable the enemy AI script temporarily
        if (newEnemy.TryGetComponent<EnemyAI>(out var enemyAI)) 
        {
            enemyAI.enabled = false;
        }
        

        // Enable the enemy AI script after a short delay
        StartCoroutine(EnableEnemyAI(enemyAI, enemyActivationDelay));
    }

    private IEnumerator EnableEnemyAI(EnemyAI enemyAI, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (enemyAI != null)
        {
            enemyAI.enabled = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawCube(transform.position, spawnAreaSize);
    }
}
