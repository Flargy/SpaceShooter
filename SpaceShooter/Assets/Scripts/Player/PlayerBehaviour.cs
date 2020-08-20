using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float currentFireRate = 0f;
    [SerializeField] private int baseDamage = 1;
    [SerializeField] private int playerHealth = 3;
    [SerializeField] private GameObject laserProjectile;
    [SerializeField] private GameObject missileProjectile;
    [SerializeField] private GameObject targetMissileProjectile;
    [SerializeField] private GameObject droneProjectile;
    [SerializeField] private Transform projectileSpawn;
    [SerializeField] private List<Transform> SpreadSpawPoints = new List<Transform>();
    [SerializeField] private List<Transform> missileSpawnpoints = new List<Transform>();

    private Dictionary<PowerUpEnums.PowerEnum, int> upgrades = new Dictionary<PowerUpEnums.PowerEnum, int>();

    private int missileCounter = 0;
    private float currentDamage = 0f;
    private Vector3 direction = Vector3.zero;
    private int fireRate = 1;
    private float cooldownTimer = 0f;
    private float immortalityTimer = 0f;
    


    void Awake()
    {
        upgrades.Add(PowerUpEnums.PowerEnum.SPREAD, 0);
        upgrades.Add(PowerUpEnums.PowerEnum.MISSILE, 0);
        upgrades.Add(PowerUpEnums.PowerEnum.DRONE, 0);
        GameVariables.Player = this;
        GameVariables.PlayerTransform = transform;
        currentDamage = baseDamage;
        currentFireRate = fireRate;
    }

    void Update()
    {
        direction.x = Input.GetAxis("Horizontal");
        direction.z = Input.GetAxis("Vertical");
        cooldownTimer += GameVariables.GameTime;
        if (Input.GetKey(KeyCode.Space))
        {
            Fire();
        }

        if (Input.GetKey(KeyCode.P))
        {
            ReceiveDamage();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            upgrades[PowerUpEnums.PowerEnum.SPREAD]++;
            upgrades[PowerUpEnums.PowerEnum.MISSILE]++;
            //upgrades[PowerUpEnums.PowerEnum.DRONE]++;
            Debug.Log("Vale upgrae to :" + upgrades[PowerUpEnums.PowerEnum.DRONE]);
        }
        Move();

        immortalityTimer += immortalityTimer < 1.5f ? GameVariables.GameTime : 0f;
    }

    private void CreateProjectile(GameObject projectile, Transform spawnPoint)
    {
        GameObject newProjectile = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
        newProjectile.GetComponent<ProjectileBase>().Damage = currentDamage;
    }

    private void InitializeProjectile(GameObject projectile, Transform spawnPoint)
    {
        if(projectile != null)
        {
            projectile.transform.position = spawnPoint.position;
            projectile.transform.rotation = spawnPoint.rotation;
            projectile.SetActive(true);
            projectile.GetComponent<ProjectileBase>().Damage = currentDamage;
        }
        else
        {
            CreateProjectile(projectile, spawnPoint);
        }
    }

    private void InitializeMissile(GameObject projectile, Transform spawnPoint)
    {
        InitializeProjectile(projectile, spawnPoint);
        projectile.GetComponent<MissileBase>().Spawn();
    }

    private void Fire()
    {
        if(cooldownTimer >= currentFireRate)
        {
            missileCounter += 1;
            InitializeProjectile(ObjectPool.Instance.GetPooledLazer(), projectileSpawn);
            int index = 0;
            for(int i = 1; i <= upgrades[PowerUpEnums.PowerEnum.SPREAD]; i++)
            {
                InitializeProjectile(ObjectPool.Instance.GetPooledLazer(), SpreadSpawPoints[index]);
                index++;
                InitializeProjectile(ObjectPool.Instance.GetPooledLazer(), SpreadSpawPoints[index]);
                index++;
                if (index >= 6)
                    break;
            }
            index = 0;
            if(missileCounter >=5 && upgrades[PowerUpEnums.PowerEnum.MISSILE] > 0)
            {
                for (int i = 1; i <= upgrades[PowerUpEnums.PowerEnum.MISSILE]; i++)
                {
                    if(index <= 1)
                    {
                        InitializeMissile(ObjectPool.Instance.GetPooledMisslie(), missileSpawnpoints[index]);
                        index++;
                        InitializeMissile(ObjectPool.Instance.GetPooledMisslie(), missileSpawnpoints[index]);
                        index++;
                    }
                    else
                    {
                        InitializeMissile(ObjectPool.Instance.GetPooledHomingMissile(), missileSpawnpoints[index]);
                        index++;
                        InitializeMissile(ObjectPool.Instance.GetPooledHomingMissile(), missileSpawnpoints[index]);
                        index++;
                    }


                    if (index >= 6)
                        break;
                }
                missileCounter = 0;
            }

            cooldownTimer = 0;
        }
    }

    private void Move()
    {

        transform.position += direction * movementSpeed * GameVariables.GameTime;

    }

    public void PowerUp(PowerUpEnums.PowerEnum powerEnum)
    {
        if (upgrades.ContainsKey(powerEnum))
        {
            upgrades[powerEnum]++;
            Debug.Log("Upgraded " + powerEnum.ToString());
        }
        else if(powerEnum == PowerUpEnums.PowerEnum.DAMAGE)
        {
            currentDamage += 0.1f * baseDamage;
            Debug.Log("Upgraded " + powerEnum.ToString());

        }
        else if (powerEnum == PowerUpEnums.PowerEnum.FIRERATE)
        {
            currentFireRate -= 0.05f * fireRate;
            Debug.Log("Upgraded " + powerEnum.ToString());

        }
    }

    public void ReceiveDamage()
    {
        if (immortalityTimer <= 1.5f)
        {
            Debug.Log("no damage");
            return;
        }
        Debug.Log("damage");

        immortalityTimer = 0f;
        playerHealth--;
        if(playerHealth == 0)
        {
            Debug.Log("i dedad, you suck");
        }
    }
}



