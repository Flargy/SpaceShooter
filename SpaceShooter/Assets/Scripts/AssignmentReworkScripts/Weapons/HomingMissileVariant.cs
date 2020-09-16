using UnityEngine;

public class HomingMissileVariant : MissileWeaponBase
{

    public override void Shoot(float damage)
    {
        currentDamage = damage;
        foreach(Transform point in firePoints)
        {
            base.InitializeProjectile(ObjectPool.Instance.GetPooledHomingMissile(), point);
        }
    }


    public override void Upgrade()
    {
        if (firePoints.Count < spawnPoints.Count)
        {
            upgradeCounter++;
            firePoints.Add(spawnPoints[upgradeCounter]);
        }
    }

    public override void ResetWeapon()
    {
        firePoints.Clear();
        upgradeCounter = 0;
        firePoints.Add(spawnPoints[upgradeCounter]);
    }
}
