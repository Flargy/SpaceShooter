using System.Collections.Generic;
using UnityEngine;

public class MissileWeaponBase : MonoBehaviour, IWeapon
{
    [SerializeField] protected List<Transform> spawnPoints = new List<Transform>();

    protected List<Transform> firePoints = new List<Transform>();
    protected int upgradeCounter = 0;
    protected float currentDamage = 0;

    private void Start()
    {
        firePoints.Add(spawnPoints[upgradeCounter]);
    }

    public virtual void Shoot(float damage)
    {
        currentDamage = damage;
       foreach(Transform point in firePoints)
        {
                InitializeProjectile(ObjectPool.Instance.GetPooledMisslie(), point);
        }

    }

  
    public virtual void InitializeProjectile(GameObject projectile, Transform spawnPoint)
    {
        if (projectile != null)
        {
            projectile.transform.position = spawnPoint.position;
            projectile.transform.rotation = spawnPoint.rotation;
            projectile.SetActive(true);
            projectile.GetComponent<ProjectileBase>().Damage = currentDamage;
        }


        projectile.GetComponent<MissileBase>().Spawn();
    }

    public virtual void Upgrade()
    {
        if(firePoints.Count < spawnPoints.Count)
        {
            upgradeCounter++;
            firePoints.Add(spawnPoints[upgradeCounter]);
            upgradeCounter++;
            firePoints.Add(spawnPoints[upgradeCounter]);
        }
    }

   public virtual void ResetWeapon()
    {
        firePoints.Clear();
        upgradeCounter = 0;
        firePoints.Add(spawnPoints[upgradeCounter]);
    }

}
