using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

//[CreateAssetMenu(menuName = "ProjectileBase")]
public class ProjectileBase : MonoBehaviour //ScriptableObject
{
    [SerializeField] protected LayerMask allowedLayers;
    [field: SerializeField] protected float projectileSpeed { get; set; }
    [field: SerializeField] protected Transform projectileTransform { get; set; }
    [field: SerializeField] protected float damageMultiplier { get; set; }
    [field: SerializeField] protected AudioController.ClipName audioType;
    [field: SerializeField] protected float audioStrength = 1.0f;
    protected float StartSpeed { get; set; }

    public float Damage { get; set; }

    public enum Type { Lazer, Missile, HomingMisslie }

    public Type ProjectileType;

    protected RaycastHit hit;

    protected SphereCollider sphere;

    [SerializeField] private bool playerUse = false;

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
        
        Physics.SphereCast(projectileTransform.position, sphere.radius, projectileTransform.forward, out hit, distance, allowedLayers);
        return hit.collider == null ? true : false; 
    }

    protected virtual void KillProjectile()
    {
        DamageableObject objectHiit = hit.collider.gameObject.GetComponent<DamageableObject>();
        if (objectHiit != null)
        {
            objectHiit.TakeDamage(Damage * damageMultiplier);
            if (playerUse)
            {
                ObjectPool.Instance.AddToList(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Debug.Log("nothing found");
        }

        GameVariables.Instance.RemoveProjectile(projectileTransform.gameObject);
    }

    public void OnEnable()
    {
        AudioController.Instance.GenerateAudio(audioType, transform.position, audioStrength);
    }

}
