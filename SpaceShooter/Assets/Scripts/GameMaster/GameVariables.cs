using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class GameVariables : MonoBehaviour
{
    private static GameVariables instance = null;

    public static GameVariables Instance { get { return instance; } }

    static public float GameTime { get; private set; }
    static public Transform PlayerTransform { get; set; }
    static public PlayerBehaviour Player { get; set; }

    static private List<GameObject> enemies = new List<GameObject>();

    static private List<GameObject> projectiles = new List<GameObject>();
    [SerializeField] private List<Material> materials = new List<Material>();
    static public List<Material> PowerUpMaterials = new List<Material>();
    static public GameUI gameUI;

    [field: SerializeField] private GameObject powerUpPrefab;
    static public GameObject PowerUpPrefab { get; private set; }

    static public bool gameRunning { get; set; } = false;


    static public Transform GetEnemy()
    {
        if (enemies.Count > 0)
            return enemies[0].transform;
        else
            return null;
    }

    private void Awake()
    {
        PowerUpPrefab = powerUpPrefab;
        if(instance == null)
        {
            instance = this;
        }
        PowerUpMaterials = materials;
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

    public void PlayerTookDamage()
    {
        gameUI.UpdatePlayerHealth();
    } 

    public void RegisterEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
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
        projectiles.Add(projectile);
    }

    public void RemoveProjectile(GameObject projectile)
    {
        if (projectiles.Contains(projectile))
        {
            projectiles.Remove(projectile);
        }
    }
}
