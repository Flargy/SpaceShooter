using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameVariables : MonoBehaviour
{
    static public float GameTime { get; set; }
    static public Transform PlayerTransform { get; set; }
    static public PlayerBehaviour Player { get; set; }

    static private List<GameObject> enemies = new List<GameObject>();

    static private List<GameObject> projectiles = new List<GameObject>();


    private void Update()
    {
        GameTime = Time.deltaTime;
    }

    static private void RegisterEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }

    static public void RemoveEnemy(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }

    static public void EmptyList(ref List<GameObject> list)
    {
        foreach(GameObject obj in list)
        {
            Destroy(obj, 0.1f);
        }
        list.Clear();
    }

    static public void RegisterProjectile(GameObject projectile)
    {
        projectiles.Add(projectile);
    }

    static public void RemoveProjectile(GameObject projectile)
    {
        if (projectiles.Contains(projectile))
        {
            projectiles.Remove(projectile);
        }
    }
}
