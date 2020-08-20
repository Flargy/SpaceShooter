using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

//[CreateAssetMenu(menuName = "ProjectileBase")]
public class ProjectileBase : MonoBehaviour //ScriptableObject
{
    [field: SerializeField] protected float projectileSpeed { get; set; }
    [field: SerializeField] protected Transform projectileTransform { get; set; }
    [field: SerializeField] protected float damageMultiplier { get; set; }
    protected float StartSpeed { get; set; }

    public float Damage { get; set; }

    public enum Type { Lazer, Missile, HomingMisslie }

    public Type ProjectileType;


    protected RaycastHit hit;

    private SphereCollider sphere;

    protected virtual void Awake()
    {
        sphere = GetComponent<SphereCollider>();
        StartSpeed = projectileSpeed;
        GameVariables.Instance.RegisterProjectile(gameObject);
    }

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
        
        Physics.SphereCast(projectileTransform.position, sphere.radius, projectileTransform.forward, out hit, distance);
        return hit.collider == null ? true : false; 
    }

    protected virtual void KillProjectile()
    {
        EnemyBase enemy = hit.collider.gameObject.GetComponent<EnemyBase>();
        PlayerBehaviour player = hit.collider.gameObject.GetComponent<PlayerBehaviour>();
        if (enemy != null)
        {
            enemy.TakeDamage(Damage * damageMultiplier);
            ObjectPool.Instance.AddToList(gameObject);

            return;
        }
        else if(player != null)
        {
            player.ReceiveDamage();
            Destroy(projectileTransform.gameObject);
            Destroy(this);
        }
        else
        {
            Debug.Log("nothing found");
        }

        GameVariables.Instance.RemoveProjectile(projectileTransform.gameObject);
    }

}
