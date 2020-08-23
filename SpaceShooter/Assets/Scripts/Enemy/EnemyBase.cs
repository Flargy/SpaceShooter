using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EnemyBase : DamageableObject
{
    [SerializeField] protected LayerMask allowedLayers;
    [SerializeField] protected float movementSpeed = 5;
    [SerializeField] protected int scoreValue = 10;

    [SerializeField] protected GameObject projectile = null;
    [SerializeField] protected List<Transform> projectileFirePoints = new List<Transform>();

    [SerializeField] protected float fireRate = 5;

    [SerializeField] protected AudioController.ClipName audioType;
    [SerializeField] protected float audioStrength = 1.0f;

    [Header("A value from 1-100")]
    [SerializeField] protected int powerUpSpawnrate = 25;

    protected float coolDownTimer = 0;
    protected float colliderRadius = 0;
    protected float startHealth = 0;

    protected RaycastHit hit;
    // Start is called before the first frame update
    protected override void Start()
    {
        if (projectileFirePoints.Count == 0)
        {
            projectileFirePoints.Add(transform);
        }
        float modifier = EnemySpawner.Instance.DifficultyMultiplier;
        health = health * modifier;
        startHealth = health;
        fireRate = fireRate * (1 - 0.05f * modifier);
        GameVariables.Instance.RegisterEnemy(this);
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
            AudioController.Instance.GenerateAudio(audioType, transform.position, audioStrength);
            ParticleSpawner.Instance.SpawnParticleEffect(ParticleSpawner.Particles.Explosion, transform.position);
            SpawnPowerup();
            KilledByPlayer();
            EnemySpawner.Instance.RemoveEnemy();
            GameVariables.Instance.RemoveEnemy(this);
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
        bool IHit = hit.collider == null ? true : false;

        if (!IHit)
        {
            Debug.Log("I have hit something on object " + hit.collider.name);
            DamageableObject hitTarget = hit.collider.GetComponent<DamageableObject>();
            if(hitTarget != null)
            {
                hitTarget.TakeDamage(1);
                if (health <= 5)
                {
                    TakeDamage(1000);
                }
                TakeDamage(1);
            }
        }

        return IHit;
    }

    protected void SpawnPowerup()
    {
        if(Random.Range(0, 100) > 100 - powerUpSpawnrate)
        {
            GameObject powerUp = Instantiate(GameVariables.PowerUpPrefab, transform.position, transform.rotation);
        }
    }

    protected void KilledByPlayer()
    {
        GameVariables.GameUI.UpdatePlayerScore(scoreValue);
        
    }

    public void GameOver()
    {
        Destroy(gameObject);
    }
}
