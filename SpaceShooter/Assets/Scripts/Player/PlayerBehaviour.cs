﻿using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PlayerBehaviour : DamageableObject
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float fireRate = 1;
    [SerializeField] private int baseDamage = 1;
    [SerializeField] private Transform projectileSpawn = default;
    [SerializeField] private List<Transform> SpreadSpawPoints = new List<Transform>();
    [SerializeField] private List<Transform> missileSpawnpoints = new List<Transform>();
    [SerializeField] private Transform droneHolder = default;
    [SerializeField] private List<PlayerDrone> drones = new List<PlayerDrone>();
    [SerializeField] private GameObject playerMesh = null;
    [SerializeField] private bool multipleUpgradesAllowed = false;

    private Dictionary<PowerUpEnums.PowerEnum, int> upgrades = new Dictionary<PowerUpEnums.PowerEnum, int>();

    private int missileCounter = 0;
    private float currentFireRate = 0f;
    private float currentDamage = 0f;
    private Vector3 direction = Vector3.zero;
    private float cooldownTimer = 0f;
    private float immortalityTimer = 0f;
    private Vector3 startPos = Vector3.zero;
    private float startHealth = 0;
    private MeshRenderer playerMeshRenderer = null;
    
    protected override void Awake()
    {
        upgrades.Add(PowerUpEnums.PowerEnum.SPREAD, 0);
        upgrades.Add(PowerUpEnums.PowerEnum.MISSILE, 0);
        upgrades.Add(PowerUpEnums.PowerEnum.DRONE, 0);
        GameVariables.Player = this;
        GameVariables.PlayerTransform = transform;
        currentDamage = baseDamage;
        currentFireRate = fireRate;
        startPos = transform.position;
        startHealth = health;
        playerMeshRenderer = playerMesh.GetComponent<MeshRenderer>();
    }

    protected override void Start()
    {
        GameVariables.GameUI.UpdateUpgrades(PowerUpEnums.PowerEnum.SPREAD, upgrades[PowerUpEnums.PowerEnum.SPREAD]);
        GameVariables.GameUI.UpdateUpgrades(PowerUpEnums.PowerEnum.MISSILE, upgrades[PowerUpEnums.PowerEnum.MISSILE]);
        GameVariables.GameUI.UpdateUpgrades(PowerUpEnums.PowerEnum.DRONE, upgrades[PowerUpEnums.PowerEnum.DRONE]);
        GameVariables.GameUI.UpdateUpgrades(PowerUpEnums.PowerEnum.DAMAGE, currentDamage);
        GameVariables.GameUI.UpdateUpgrades(PowerUpEnums.PowerEnum.FIRERATE, currentFireRate);
    }

    protected override void Update()
    {
        if (!GameVariables.gameRunning)
        {
            return;
        }

        direction.x = Input.GetAxis("Horizontal");
        direction.z = Input.GetAxis("Vertical");

        droneHolder.RotateAround(droneHolder.transform.position, transform.up, 90 * GameVariables.GameTime);

        playerMesh.transform.rotation = Quaternion.Euler(new Vector3(direction.x * 10, 0, direction.z * 10));

        cooldownTimer += GameVariables.GameTime;
        if (Input.GetKey(KeyCode.Space))
        {
            Fire();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameVariables.GameUI.PauseGame();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            PowerUp(PowerUpEnums.PowerEnum.SPREAD);
            PowerUp(PowerUpEnums.PowerEnum.MISSILE);
            PowerUp(PowerUpEnums.PowerEnum.DRONE);
        }

        Move();
        immortalityTimer += immortalityTimer < 2.1f ? GameVariables.GameTime : 0f;

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

            FireLaser();
            if (missileCounter >= 5 && upgrades[PowerUpEnums.PowerEnum.MISSILE] > 0)
            {
                FireMissile();
            }

            DroneFire();
            cooldownTimer = 0;
        }
    }

    private void FireLaser()
    {
        int index = 0;
        for (int i = 1; i <= upgrades[PowerUpEnums.PowerEnum.SPREAD]; i++)
        {
            InitializeProjectile(ObjectPool.Instance.GetPooledLazer(), SpreadSpawPoints[index]);
            index++;
            InitializeProjectile(ObjectPool.Instance.GetPooledLazer(), SpreadSpawPoints[index]);
            index++;
            if (index >= SpreadSpawPoints.Count)
            {
                break;
            }
        }
    }

    private void FireMissile()
    {
        int index = 0;

            for (int i = 1; i <= upgrades[PowerUpEnums.PowerEnum.MISSILE]; i++)
            {
                if (index <= 1)
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

                if (index >= missileSpawnpoints.Count)
                    break;
            }
        missileCounter = 0;
    }

    private void DroneFire()
    {
        foreach(PlayerDrone drone in drones)
        {
            drone.Fire(currentDamage);
        }
    }

    private void Move()
    {
        Vector3 movementVector = transform.position + direction * movementSpeed * GameVariables.GameTime;
        if (GameBoundaries.Instance.InsideBoundaries(movementVector) == false)
        {
            
            movementVector = GameBoundaries.Instance.GetLocationInBoundary(movementVector);
        }
        
        transform.position = movementVector;
    }

    public void PowerUp(PowerUpEnums.PowerEnum powerEnum)
    {
        Debug.Log("Powerup added by type: " + powerEnum.ToString());
        if (upgrades.ContainsKey(powerEnum))
        {
            int currentUppgrade = 0;

            if (powerEnum == PowerUpEnums.PowerEnum.DRONE)
            {
                currentUppgrade = upgrades[powerEnum]++;
                GameVariables.GameUI.UpdateUpgrades(powerEnum, upgrades[powerEnum]);
                foreach (PlayerDrone drone in drones)
                {
                    if (!drone.Active)
                    {
                        drone.ActivateDrone(true);
                        return;
                    }
                }
            }
            else
            {
                currentUppgrade = upgrades[powerEnum]++;
                GameVariables.GameUI.UpdateUpgrades(powerEnum, upgrades[powerEnum]);
            }

            if(multipleUpgradesAllowed == false)
            {
                foreach (PowerUpEnums.PowerEnum entry in Enum.GetValues(typeof(PowerUpEnums.PowerEnum)))
                {
                    if (upgrades.ContainsKey(entry))
                    {
                        if (entry == powerEnum)
                        {
                            upgrades[entry] = currentUppgrade;
                            GameVariables.GameUI.UpdateUpgrades(entry, upgrades[entry]);
                        }
                        else
                        {
                            upgrades[entry] = 0;
                            GameVariables.GameUI.UpdateUpgrades(entry, upgrades[entry]);
                        }
                    }
                }
            }
        }
        else
        {
            if (powerEnum == PowerUpEnums.PowerEnum.DAMAGE)
            {
                currentDamage += 0.1f * baseDamage;
                GameVariables.GameUI.UpdateUpgrades(powerEnum, currentDamage);
            }
            else if (powerEnum == PowerUpEnums.PowerEnum.FIRERATE)
            {
                currentFireRate -= 0.05f * fireRate;
                GameVariables.GameUI.UpdateUpgrades(powerEnum, currentDamage);
            }

        }
    }

    public override void TakeDamage(float dmg)
    {
        if (immortalityTimer <= 2f)
        {
            return;
        }

        immortalityTimer = 0f;
        StartCoroutine(flashForImmunity());
        health--;
        GameVariables.GameUI.UpdatePlayerHealth();
        if (health == 0)
        {
            AudioController.Instance.GenerateAudio(AudioController.ClipName.PlayerDestroyed, transform.position, 0.1f);
            GameVariables.Instance.ResetTheGame();
        }
    }

    public void ResetPlayer()
    {
        upgrades.Clear();
        upgrades.Add(PowerUpEnums.PowerEnum.SPREAD, 0);
        upgrades.Add(PowerUpEnums.PowerEnum.MISSILE, 0);
        upgrades.Add(PowerUpEnums.PowerEnum.DRONE, 0);
        GameVariables.Player = this;
        GameVariables.PlayerTransform = transform;
        health = startHealth;
        currentDamage = baseDamage;
        currentFireRate = fireRate;
        transform.position = startPos;
        foreach (PlayerDrone drone in drones)
        {
            drone.ActivateDrone(false);
        }

        GameVariables.GameUI.UpdateUpgrades(PowerUpEnums.PowerEnum.SPREAD, upgrades[PowerUpEnums.PowerEnum.SPREAD]);
        GameVariables.GameUI.UpdateUpgrades(PowerUpEnums.PowerEnum.MISSILE, upgrades[PowerUpEnums.PowerEnum.MISSILE]);
        GameVariables.GameUI.UpdateUpgrades(PowerUpEnums.PowerEnum.DRONE, upgrades[PowerUpEnums.PowerEnum.DRONE]);
        GameVariables.GameUI.UpdateUpgrades(PowerUpEnums.PowerEnum.DAMAGE, currentDamage);
        GameVariables.GameUI.UpdateUpgrades(PowerUpEnums.PowerEnum.FIRERATE, currentFireRate);
    }

    private IEnumerator flashForImmunity()
    {
        float gameTimer = GameVariables.GameTime * 1;
        while (immortalityTimer < 1.9f)
        {
            playerMeshRenderer.enabled = false;
            yield return new WaitForSeconds(gameTimer * 0.1f);
            playerMeshRenderer.enabled = true;
            yield return new WaitForSeconds(gameTimer * 0.1f);
        }
        yield return null;
    }

}



