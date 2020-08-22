using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankUnit : EnemyBase
{
    protected override void Update()
    {
        Movmentbehaviour();
        coolDownTimer += GameVariables.GameTime;
        if (coolDownTimer > fireRate)
        {
            Fire();
            coolDownTimer = 0;
        }
    }

    protected override void Movmentbehaviour()
    {
        float distance = movementSpeed * GameVariables.GameTime;
        if (transform.position.z <= 0)
        {
            transform.LookAt(GameVariables.PlayerTransform);
        }
        else if (CheckCollision(transform.forward, distance))
        {
            transform.position += transform.forward.normalized * distance;
        }
    }

    protected override void Fire()
    {
        foreach (Transform trans in projectileFirePoints)
        {
            GameObject pew = Instantiate(projectile, trans.position, trans.rotation);
        }
    }
}
