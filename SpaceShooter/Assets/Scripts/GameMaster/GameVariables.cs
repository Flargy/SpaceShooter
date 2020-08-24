using System.Collections.Generic;
using UnityEngine;

public class GameVariables : MonoBehaviour
{
    private static GameVariables instance = null;

    public static GameVariables Instance { get { return instance; } }

    static public float GameTime { get; private set; } = 0f;
    static public Transform PlayerTransform { get; set; } = default;
    static public PlayerBehaviour Player { get; set; } = null;

    //static private List<GameObject> enemies = new List<GameObject>();
    static private List<DamageableObject> enemies = new List<DamageableObject>();
    static private List<GameObject> enemyProjectiles = new List<GameObject>();

    [SerializeField] private List<GameObject> powerUps = new List<GameObject>();
    static public List<GameObject> PowerUps = new List<GameObject>();
    [SerializeField] private GameUI gameUI = null;
    static public GameUI GameUI { get; private set; }

    [field: SerializeField] private GameObject powerUpPrefab = null;
    static public GameObject PowerUpPrefab { get; private set; } = null;

    static public bool gameRunning { get; set; } = false;

    private void Awake()
    {
        PowerUpPrefab = powerUpPrefab;
        if(instance == null)
        {
            instance = this;
        }
        PowerUps = powerUps;
        GameUI = gameUI;
    }

    private void Update()
    {
        if (gameRunning)
        {
            GameTime = Time.deltaTime;
        }
        else
        {
            GameTime = 0;
        }
    }

    public Transform GetEnemy()
    {
        if (enemies.Count != 0)
            return enemies[0].transform;
        else
            return null;
    }

    public void RegisterEnemy(DamageableObject enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveEnemy(DamageableObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemy.DestroyMyGameObject();
            enemies.Remove(enemy);
        }
    }

    public void EmptyList(ref List<GameObject> list)
    {
        foreach(GameObject obj in list)
        {
            Destroy(obj, 0.1f);
        }

        list.Clear();
    }

    public void RegisterProjectile(GameObject projectile)
    {
        enemyProjectiles.Add(projectile);
    }

    public void RemoveProjectile(GameObject projectile)
    {
        if (enemyProjectiles.Contains(projectile))
        {
            enemyProjectiles.Remove(projectile);
        }
    }

    public void ResetTheGame()
    {
        gameRunning = false;
        foreach(DamageableObject enemy in enemies)
        {
            if(enemy != null && enemy.gameObject != null)
            {
                enemy.DestroyMyGameObject();
            }
        }

        enemies.Clear();
        gameUI.GameOver();
        Player.ResetPlayer();
    }
}
