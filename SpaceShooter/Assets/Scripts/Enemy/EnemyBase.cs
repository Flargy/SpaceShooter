using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EnemyBase : DamageableObject
{
    [SerializeField] protected LayerMask allowedLayers;
    [SerializeField] protected int scoreValie = 0;
    [SerializeField] protected float movementSpeed = 5;

    [SerializeField] protected GameObject projectile = null;
    [SerializeField] protected List<Transform> projectileFirePoints = new List<Transform>();

    [SerializeField] protected float fireRate = 5;

    [Header("A value from 1-100")]
    [SerializeField] protected int powerUpSpawnrate = 25;

    protected float coolDownTimer = 0;
    protected float colliderRadius = 0;

    protected RaycastHit hit;
    // Start is called before the first frame update
    protected override void Start()
    {
        float modifier = EnemySpawner.Instance.DifficultyMultiplier;
        health = health * modifier;
        fireRate = fireRate - 1 * 0.2f * modifier;
        GameVariables.Instance.RegisterEnemy(gameObject);
        colliderRadius = GetComponent<SphereCollider>().radius;
    }

    // Update is called once per frame
    protected override void Update()
    {
        Movmentbehaviour();
        coolDownTimer += GameVariables.GameTime;
        if(coolDownTimer > fireRate)
        {
            Fire();
            coolDownTimer = 0;
        }
    }

    public override void TakeDamage(float dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            EnemySpawner.Instance.RemoveEnemy();
            Destroy(gameObject);
        }
    }

    protected virtual void Fire()
    {
        foreach(Transform trans in projectileFirePoints)
        {
            GameObject pew = Instantiate(projectile, trans.position, trans.rotation);
            pew.transform.LookAt(GameVariables.PlayerTransform);
        }

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
