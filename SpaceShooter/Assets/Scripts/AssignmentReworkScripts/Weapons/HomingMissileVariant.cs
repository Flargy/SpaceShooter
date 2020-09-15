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
        //fix this later
    }
}
