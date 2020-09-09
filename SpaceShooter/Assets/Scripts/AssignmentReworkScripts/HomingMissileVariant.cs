﻿using UnityEngine;

public class HomingMissileVariant : MissileWeaponBase
{

    public override void Shoot(float damage)
    {
        currentDamage = damage;
        foreach(Transform point in spawnPoints)
        {
            base.InitializeProjectile(ObjectPool.Instance.GetPooledHomingMissile(), point);
        }
    }
   
}
