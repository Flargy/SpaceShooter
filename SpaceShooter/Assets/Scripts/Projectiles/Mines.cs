using UnityEngine;

public class Mines : DamageableObject
{
    [SerializeField] private float moveSpeed = 2;
    [SerializeField] private Transform rotator;
    [SerializeField] private Transform[] firePoints;
    [SerializeField] private float fireRate = 1;
    [SerializeField] private GameObject projectile = null;
    public BossBase parentBoss;

    private float fireTimer = 0;

    protected override void Update()
    {
        fireTimer += GameVariables.GameTime;
        if(fireTimer >= fireRate)
        {
            Fire();
        }
        transform.position += transform.forward * moveSpeed * GameVariables.GameTime;

        rotator.RotateAround(transform.position, transform.up, 0.2f);
    }

    void Fire()
    {
        foreach(Transform trans in firePoints)
        {
            GameObject pew = Instantiate(projectile, trans.position, trans.rotation);
        }
        fireTimer = 0;
    }

    public override void TakeDamage(float dmg)
    {
        health--;
        if (health == 0)
        {
            parentBoss.listOfMines.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
