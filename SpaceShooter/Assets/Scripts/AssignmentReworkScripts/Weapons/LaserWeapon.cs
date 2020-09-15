using System.Collections.Generic;
using UnityEngine;

public class LaserWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

    private List<Transform> firePoints = new List<Transform>();
    private int upgradeCounter = 0;

    private float currentDamage = 0.0f;

    private void Start()
    {
        firePoints.Add(spawnPoints[upgradeCounter]);
    }

    public void Shoot(float damage)
    {
        currentDamage = damage;
        foreach(Transform point in firePoints)
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

    public void Upgrade()
    {
        if(firePoints.Count < spawnPoints.Count)
        {
            upgradeCounter++;
            firePoints.Add(spawnPoints[upgradeCounter]);
            upgradeCounter++;
            firePoints.Add(spawnPoints[upgradeCounter]);

        }
    }

    public void ResetWeapon()
    {
        firePoints.Clear();
        upgradeCounter = 0;
        firePoints.Add(spawnPoints[upgradeCounter]);
    }
}
