using System.Collections.Generic;
using UnityEngine;

public class MissileWeaponBase : MonoBehaviour, IWeapon
{
    [SerializeField] protected List<Transform> spawnPoints = new List<Transform>();

    protected float currentDamage = 0;

    public virtual void Shoot(float damage)
    {
        currentDamage = damage;
       foreach(Transform point in spawnPoints)
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

   

}
