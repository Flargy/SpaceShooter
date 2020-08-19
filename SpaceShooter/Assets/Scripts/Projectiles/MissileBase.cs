using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBase : ProjectileBase
{

    [SerializeField] private float accelerate;
    // Start is called before the first frame update
    void Start()
    {
    }

    protected override void Update()
    {
        Move();
    }

    protected override void Move()
    {
        base.projectileSpeed *= accelerate;
        if (base.CheckCollision(projectileSpeed * GameVariables.GameTime))
        {
            Debug.Log("ProjectileSpeed: " + projectileSpeed);
            transform.position += transform.forward.normalized * projectileSpeed * GameVariables.GameTime;
        }
        else
        {
            KillProjectile();
        }
    }
}
