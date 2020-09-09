using System.Collections.Generic;
using UnityEngine;

public class LaserWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

    private float currentDamage = 0.0f;

    public void Shoot(float damage)
    {
        currentDamage = damage;
        foreach(Transform point in spawnPoints)
        {
            InitializeProjectile(ObjectPool.Instance.GetPooledLazer(), point);
        }

    }

    public void InitializeProjectile(GameObject projectile, Transform spawnPoint)
    {
        if (projectile != null)
        {
            projectile.transform.position = spawnPoint.position;
            projectile.transform.rotation = spawnPoint.rotation;
            projectile.SetActive(true);
            projectile.GetComponent<ProjectileBase>().Damage = currentDamage;
        }

    }

}
