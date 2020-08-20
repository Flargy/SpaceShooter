using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKamikaze : EnemyBase
{
    [SerializeField] private float targetDistance = 8;
    [SerializeField] private float aimTimer = 3;
    [SerializeField] private float speedMultiplier = 2.5f;
    private bool locked = false;
    private bool charge = false;
    private float chargeTimer = 0;

    protected override void Update()
    {
        if (locked)
        {
            if (charge)
            {
                Movmentbehaviour();
            }
            else
            {
                transform.LookAt(GameVariables.PlayerTransform);
                chargeTimer += GameVariables.GameTime;
                if (chargeTimer >= aimTimer)
                {
                    charge = true;
                    movementSpeed *= speedMultiplier;
                }
            }
        }
        else
        {
            base.Update();
            locked = CheckDistanceToPlayer();
        }
    }

    protected override void Movmentbehaviour()
    {
        float distance = movementSpeed * GameVariables.GameTime;
        if (CheckCollision(transform.forward, distance))
        {
            transform.position += transform.forward.normalized * distance;
        }
    }

    private bool CheckDistanceToPlayer()
    {
        Vector3 distance = transform.position - GameVariables.PlayerTransform.position;
        if(distance.magnitude < targetDistance)
        {
            return true;
        }
        return false;
    }



}
