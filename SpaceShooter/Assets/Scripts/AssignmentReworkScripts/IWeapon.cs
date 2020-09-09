using UnityEngine;
public interface IWeapon
{
    void Shoot(float damage);

    void InitializeProjectile(GameObject projectile, Transform spawnPoint);
}
