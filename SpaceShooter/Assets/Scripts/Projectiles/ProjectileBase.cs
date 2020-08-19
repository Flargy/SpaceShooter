﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

//[CreateAssetMenu(menuName = "ProjectileBase")]
public class ProjectileBase : MonoBehaviour //ScriptableObject
{
    [field: SerializeField] protected float projectileSpeed { get; set; }
    [field: SerializeField] protected Transform projectileTransform { get; set; }
    [field: SerializeField] public float damage { get; set; }

    RaycastHit hit;

    // Update is called once per frame
    protected virtual void Update()
    {
        Move();
    }


    protected virtual void Move()
    {
        if (CheckCollision(projectileSpeed * GameVariables.GameTime))
        {
            projectileTransform.position += projectileTransform.forward * projectileSpeed * GameVariables.GameTime;
        }
        else
        {
            KillProjectile();
        }
    }

    protected virtual bool CheckCollision(float distance)
    {
        Physics.Raycast(projectileTransform.position, projectileTransform.forward, out hit, distance);
        return hit.collider == null ? true : false; 
    }

    protected virtual void KillProjectile()
    {
        EnemyBase enemy = hit.collider.gameObject.GetComponent<EnemyBase>();
        PlayerBehaviour player = hit.collider.gameObject.GetComponent<PlayerBehaviour>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Debug.Log("Enemyfound");
        }
        else if(player != null)
        {
            player.ReceiveDamage();
            Debug.Log("Player found");
        }
        else
        {
            Debug.Log("nothing found");
        }

        GameVariables.RemoveProjectile(projectileTransform.gameObject);
        Destroy(projectileTransform.gameObject);
        Destroy(this);
    }

}
