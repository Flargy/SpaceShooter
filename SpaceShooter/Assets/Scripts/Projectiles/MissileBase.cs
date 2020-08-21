using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBase : ProjectileBase
{

    [SerializeField] private float accelerate = 0;

    protected override void Awake()
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
        if (CheckCollision(projectileSpeed * GameVariables.GameTime))
        {
            //Debug.Log("ProjectileSpeed: " + projectileSpeed);
            projectileTransform.position += projectileTransform.forward.normalized * projectileSpeed * GameVariables.GameTime;
        }
        else
        {
            //Debug.Log(hit.collider.gameObject.name);
            base.KillProjectile();
        }

        Vector3 vec = projectileTransform.position + (projectileTransform.forward.normalized * projectileSpeed * GameVariables.GameTime);

        Debug.DrawLine(transform.position, vec, Color.blue);
    }

    protected override bool CheckCollision(float distance)
    {
        Physics.SphereCast(projectileTransform.position, sphere.radius, projectileTransform.forward, out hit, distance, LayerMask.GetMask("Enemy"));
        return hit.collider == null ? true : false;
    }

    public virtual void Spawn()
    {
        projectileSpeed = StartSpeed;

    }
}
