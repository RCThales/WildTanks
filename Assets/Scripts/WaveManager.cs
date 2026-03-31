using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<EnemyUnlockData> enemyPool;
    [SerializeField] private List<EnemyUnlockData> bossPool;
    [SerializeField] private float waveDuration = 30f;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private float minSpawnRadius = 8f;
    [SerializeField] private float maxSpawnRadius = 20f;
    [SerializeField] private Transform playerPosition;

    [SerializeField] private Vector2 levelBoundsCenter = Vector2.zero;
    [SerializeField] private Vector2 levelBoundsSize = new Vector2(-36f, 36f);

    [SerializeField] TextMeshProUGUI waveTimerText;
    [SerializeField] TextMeshProUGUI waveNumberText;

    [SerializeField] private GameObject preSpawnMarkingPrefab;

    //Time Between waves
    [SerializeField] private float timeBetweenWaves = 10f;
    private bool isWaveActive = false;

    private int currentWave = 0;
    private float waveTimer = 0f;
    private float spawnTimer = 0f;


    private void Start()
    {
        StartNextWave();
    }

    private void Update()
    {

        UpdateWaveTimer();

        if (isWaveActive)
        {
            if (spawnTimer >= spawnInterval)
            {
                SpawnEnemies();
                spawnTimer = 0f;
            }

            if (waveTimer >= waveDuration)
            {
                StartBreakPhase();
            }
        }
        else
        {
            if (waveTimer >= timeBetweenWaves)
            {
                StartNextWave();
            }
        }

    }

    private void StartNextWave()
    {
        UpdateWaveNumber();
        ResetTimers();
        isWaveActive = true;

    }

    private void StartBreakPhase()
    {
        isWaveActive = false;
        waveTimer = 0f;
        waveTimerText.text = $"Break Time!";
        StopAllCoroutines();
        StartCoroutine(KillAllEnemiesWithDelay(0.01f));
        Debug.Log("Break phase started!");
    }

    private IEnumerator KillAllEnemiesWithDelay(float delay)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
            yield return new WaitForSeconds(delay);
        }

    }
    private void SpawnEnemies()
    {
        float spawnRadius;
        for (int i = 0; i < enemyPool.Count; i++)
        {
            EnemyUnlockData data = enemyPool[i];
            if (currentWave >= data.unlockAtWave)
            {
                int amountToSpawn = data.baseAmount + (currentWave - data.unlockAtWave);
                for (int j = 0; j < amountToSpawn; j++)
                {
                    spawnRadius = Random.Range(minSpawnRadius, maxSpawnRadius);
                    Vector2 spawnPosition = (Vector2)playerPosition.position +
    Random.insideUnitCircle.normalized * spawnRadius;
                    StartCoroutine(PreSpawnSequence(data, spawnPosition));
                }
            }
        }
    }

    private IEnumerator PreSpawnSequence(EnemyUnlockData data, Vector2 spawnPosition)
    {

        spawnPosition = GetSpawnPosition();
        // Show the marker
        GameObject marking = Instantiate(preSpawnMarkingPrefab, spawnPosition, Quaternion.identity);

        // Wait 2 seconds
        yield return new WaitForSeconds(2f);

        // Destroy marker and spawn enemy
        Destroy(marking);
        Instantiate(data.enemyPrefab, spawnPosition, Quaternion.identity);
    }
    private Vector2 GetSpawnPosition()
    {
        Vector2 spawnPosition;
        int maxAttempts = 10;

        do
        {
            float spawnRadius = Random.Range(minSpawnRadius, maxSpawnRadius);
            spawnPosition = (Vector2)playerPosition.position +
                Random.insideUnitCircle.normalized * spawnRadius;
            maxAttempts--;

        }
        while (!IsInsideBounds(spawnPosition) && maxAttempts > 0);

        return spawnPosition;
    }

    private bool IsInsideBounds(Vector2 position)
    {
        float halfWidth = levelBoundsSize.x / 2f;
        float halfHeight = levelBoundsSize.y / 2f;

        return position.x > levelBoundsCenter.x - halfWidth &&
               position.x < levelBoundsCenter.x + halfWidth &&
               position.y > levelBoundsCenter.y - halfHeight &&
               position.y < levelBoundsCenter.y + halfHeight;
    }
    private void ResetTimers()
    {
        waveTimer = 0f;
        spawnTimer = 0f;
    }

    private void UpdateWaveNumber()
    {
        currentWave++;
        waveNumberText.text = $"Wave {currentWave}";
    }

    private void UpdateWaveTimer()
    {
        waveTimer += Time.deltaTime;
        spawnTimer += Time.deltaTime;
        if (isWaveActive)
        {
            float remainingTime = Mathf.Max(0, waveDuration - waveTimer);
            waveTimerText.text = $"{(int)remainingTime}";
        }
        else
        {
            float breakRemaining = Mathf.Max(0, timeBetweenWaves - waveTimer);
            waveTimerText.text = $"Next wave in {(int)breakRemaining}";
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(levelBoundsCenter, levelBoundsSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerPosition.position, minSpawnRadius);
        Gizmos.DrawWireSphere(playerPosition.position, maxSpawnRadius);
    }
}

