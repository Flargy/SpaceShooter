using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private static EnemySpawner instance = null;

    public static EnemySpawner Instance { get { return instance; } }
    public float DifficultyMultiplier = 1;

    private int activeEnemies = 0;
    private int maxEnemies = 3;
    private int wave = 1;
    private int recentSpawnPoint = 1;
    [SerializeField] private int killsToBoss = 10;
    private int currentKills = 0;
    private bool bossActive = false;
    private float spawnDelay = 1.5f;
    private float spawnTimer = 0f;


    [SerializeField] private List<GameObject> enemies = new List<GameObject>();
    [SerializeField] private List<Transform> enemySpawnPoints = new List<Transform>();
    [SerializeField] private List<GameObject> bosses = new List<GameObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    private void LateUpdate()
    {
        if(bossActive == false)
        {
            SpawnEnemies();
        }
    }

    private void SpawnEnemies()
    {
        spawnTimer += GameVariables.GameTime;
        if (activeEnemies < maxEnemies && spawnTimer > spawnDelay)
        {
            AddActiveEnemy();
            int spawnPoint;
            do
            {
                spawnPoint = Random.Range(0, enemySpawnPoints.Count);
            }
            while (spawnPoint == recentSpawnPoint);

            recentSpawnPoint = spawnPoint;
            int enemy = Random.Range(0, wave) % enemies.Count;
            Instantiate(enemies[enemy], enemySpawnPoints[spawnPoint].position, enemySpawnPoints[spawnPoint].rotation);
            spawnTimer = 0;
        }
    }

    public void AddActiveEnemy()
    {
        activeEnemies++;
    }

    public void RemoveEnemy()
    {
        activeEnemies--;
        currentKills++;
        if (currentKills >= killsToBoss && bossActive == false)
        {
            SpawnBoss();
            currentKills = 0;
        }
    }

    public void EnemyOutOfBounds()
    {
        activeEnemies--;
    }

    public void BossDefeated(float waveDelayTimer)
    {
        wave++;
        killsToBoss = killsToBoss * 2;
        DifficultyMultiplier = DifficultyMultiplier * 1.5f;
        maxEnemies += 2;
        spawnTimer = 0;
        GameVariables.GameUI.UpdateWave();
        StartCoroutine(WaveDelayAfterBossKill(waveDelayTimer));
    }

    private void SpawnBoss()
    {
        bossActive = true;
        Instantiate(bosses[wave % bosses.Count], enemySpawnPoints[2].position, enemySpawnPoints[2].rotation);
    }

    private IEnumerator WaveDelayAfterBossKill(float delay)
    {
        yield return new WaitForSeconds(delay);
        bossActive = false;
    }

    public void GameReset()
    {
        activeEnemies = 0;
        maxEnemies = 3;
        wave = 1;
        currentKills = 0;
        bossActive = false;
        killsToBoss = 10;
        DifficultyMultiplier = 1.0f;
    }
}
