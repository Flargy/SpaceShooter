using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBase : ProjectileBase
{

    [SerializeField] private float accelerate;
    // Start is called before the first frame update

    protected virtual void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        Move();
    }

    protected override void Move()
    {
        base.projectileSpeed += accelerate * GameVariables.GameTime;
        if (base.CheckCollision(projectileSpeed * GameVariables.GameTime))
        {
            //Debug.Log("ProjectileSpeed: " + projectileSpeed);
            projectileTransform.position += projectileTransform.forward.normalized * projectileSpeed * GameVariables.GameTime;
        }
        else
        {
            Debug.Log(hit.collider.gameObject.name);
            base.KillProjectile();
        }

        Vector3 vec = projectileTransform.position + (projectileTransform.forward.normalized * projectileSpeed * GameVariables.GameTime);

        Debug.DrawLine(transform.position, vec, Color.blue);
    }

    public virtual void Spawn()
    {
        projectileSpeed = StartSpeed;

    }
}
