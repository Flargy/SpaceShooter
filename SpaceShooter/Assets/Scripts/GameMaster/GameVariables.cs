using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameVariables : MonoBehaviour
{
    static public float GameTime { get; set; }
    static public Transform PlayerTransform { get; set; }

    static private List<GameObject> enemies = new List<GameObject>();

    static private List<GameObject> projectiles = new List<GameObject>();

    private void Update()
    {
        GameTime = Time.deltaTime;
    }

    private void RegisterEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }

    private void RemoveEnemy(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }

    private void EmptyList(ref List<GameObject> list)
    {
        foreach(GameObject obj in list)
        {
            Destroy(obj, 0.1f);
        }
        list.Clear();
    }

    private void RegisterProjectile(GameObject projectile)
    {
        projectiles.Add(projectile);
    }

    private void RemoveProjectile(GameObject projectile)
    {
        if (projectiles.Contains(projectile))
        {
            projectiles.Remove(projectile);
        }
    }
}
