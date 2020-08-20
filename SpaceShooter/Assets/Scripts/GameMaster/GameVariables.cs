using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class GameVariables : MonoBehaviour
{
    private static GameVariables instance = null;

    public static GameVariables Instance { get { return instance; } }

    static public float GameTime { get; set; }
    static public Transform PlayerTransform { get; set; }
    static public PlayerBehaviour Player { get; set; }

    static private List<GameObject> enemies = new List<GameObject>();

    static private List<GameObject> projectiles = new List<GameObject>();
    [SerializeField] private List<Material> materials = new List<Material>();
    static public List<Material> PowerUpMaterials = new List<Material>();

    static public Transform GetEnemy()
    {
        return enemies[0].transform;
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        PowerUpMaterials = materials;
    }
    private void Update()
    {
        GameTime = Time.deltaTime;
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
