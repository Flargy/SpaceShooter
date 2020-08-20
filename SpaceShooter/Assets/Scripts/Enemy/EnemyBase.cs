using System;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected LayerMask allowedLayers;
    [SerializeField] protected float health = 10;

    [SerializeField] protected float movementSpeed = 5;

    [SerializeField] protected GameObject projectile;
    [SerializeField] protected Transform projectileFire;

    [SerializeField] protected float fireRate = 5;
    [SerializeField] protected bool isBoss = false;

    [Header("A value from 1-100")]
    [SerializeField] protected int powerUpSpawnrate;

    private float coolDownTimer = 0;
    protected float colliderRadius;

    protected RaycastHit hit;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        //float modifier = EnemySpawner.Instance.DifficultyMultiplier;
        //health = health * modifier;
        //fireRate = fireRate - 1 * 0.2f * modifier;
        GameVariables.Instance.RegisterEnemy(gameObject);
        colliderRadius = GetComponent<SphereCollider>().radius;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Movmentbehaviour();
        coolDownTimer += GameVariables.GameTime;
        if(coolDownTimer > fireRate)
        {
            Fire();
            coolDownTimer = 0;
        }
    }

    public virtual void TakeDamage(float dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            if (isBoss == false)
                EnemySpawner.Instance.RemoveEnemy();
            else
                EnemySpawner.Instance.BossDefeated();
            Destroy(gameObject);
        }
    }

    protected virtual void Fire()
    {
        GameObject pew = Instantiate(projectile, projectileFire.position, projectileFire.rotation);
        pew.transform.LookAt(GameVariables.PlayerTransform);
    }

    protected virtual void Movmentbehaviour()
    {
        float distance = movementSpeed * GameVariables.GameTime;
        if (CheckCollision(transform.forward, distance))
        {
            transform.position += transform.forward.normalized * distance;
        }
    } 

    protected virtual bool CheckCollision(Vector3 direction, float distance)
    {
        Physics.Raycast(transform.position, direction.normalized, out hit, distance + colliderRadius, allowedLayers);
        return hit.collider == null ? true : false;
    }

    protected virtual void OnDisable()
    {
        if (health <= 0 && UnityEngine.Random.Range(0, 100) > 100 - powerUpSpawnrate)
        {
            SpawnPowerup();
        }

        GameVariables.Instance.RemoveEnemy(gameObject);
    }

    protected void SpawnPowerup()
    {
        GameObject powerUp = Instantiate(GameVariables.PowerUpPrefab, transform.position, transform.rotation);
    }
}
