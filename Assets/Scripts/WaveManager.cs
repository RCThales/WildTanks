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
    [SerializeField] private GameObject spawnVFXPrefab;

    //Time Between waves
    [SerializeField] private float timeBetweenWaves = 10f;
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private ShopCameraTransition shopCameraTransition;
    private bool isWaveActive = false;
    private PlayerController playerController;
    private readonly List<GameObject> activeMarkers = new List<GameObject>();

    public int CurrentWave { get; private set; } = 0;
    private float waveTimer = 0f;
    private float spawnTimer = 0f;


    private void Start()
    {
        playerController = playerPosition.GetComponent<PlayerController>();
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
        waveTimerText.gameObject.SetActive(true);
        waveNumberText.gameObject.SetActive(true);
        playerController?.SetMovementEnabled(true);
        shopCameraTransition?.TransitionToGame();
    }

    public void StartBreakPhase()
    {
        isWaveActive = false;
        waveTimer = 0f;
        waveTimerText.text = $"Break Time!";
        StopAllCoroutines();
        foreach (GameObject marker in activeMarkers)
            if (marker != null) Destroy(marker);
        activeMarkers.Clear();
        StartCoroutine(KillAllEnemiesWithDelay(0.01f));
        waveTimerText.gameObject.SetActive(false);
        waveNumberText.gameObject.SetActive(false);
        playerController?.SetMovementEnabled(false);
        shopCameraTransition?.TransitionToShop();
        shopManager.OpenShop();
    }

    public void SkipBreak()
    {
        if (!isWaveActive)
            StartNextWave();
    }

    private IEnumerator KillAllEnemiesWithDelay(float delay)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            if (enemy == null) continue;
            if (enemy.TryGetComponent(out Health h)) h.Kill();
            yield return new WaitForSeconds(delay);
        }

    }
    private void SpawnEnemies()
    {
        float spawnRadius;
        for (int i = 0; i < enemyPool.Count; i++)
        {
            EnemyUnlockData data = enemyPool[i];
            if (CurrentWave >= data.unlockAtWave)
            {
                int amountToSpawn = data.baseAmount + (CurrentWave - data.unlockAtWave);
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
        activeMarkers.Add(marking);

        // Wait 2 seconds
        yield return new WaitForSeconds(2f);

        // Destroy marker and spawn enemy
        activeMarkers.Remove(marking);
        Destroy(marking);
        if (spawnVFXPrefab != null)
        {
            GameObject vfx = Instantiate(spawnVFXPrefab, spawnPosition, Quaternion.identity);
            foreach (var r in vfx.GetComponentsInChildren<ParticleSystemRenderer>())
                r.sortingOrder = 100;
        }
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
        CurrentWave++;
        waveNumberText.text = $"Wave {CurrentWave}";
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
            shopManager.UpdateBreakTimer(breakRemaining);
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

