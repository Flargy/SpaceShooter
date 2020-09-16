using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PlayerBehaviour : DamageableObject
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float fireRate = 1;
    [SerializeField] private int baseDamage = 1;
    [SerializeField] private Transform droneHolder = default;
    [SerializeField] private List<PlayerDrone> drones = new List<PlayerDrone>();
    [SerializeField] private GameObject playerMesh = null;
    //new stuff
    [SerializeField] private List<GameObject> weaponList = new List<GameObject>();
    [SerializeField] private GameObject playerShield = null;
    [SerializeField] private float shieldCooldown = 10.0f;
    [SerializeField] private float shieldDuration = 2.0f;
    private float shieldTimer = 0.0f;
    private ParticleSystem shieldParticleSystem = null;
    private float weaponsSpecificFireRate = 0.0f;


    private Dictionary<PowerUpEnums.PowerEnum, int> upgrades = new Dictionary<PowerUpEnums.PowerEnum, int>();

    private float currentFireRate = 0f;
    private float currentDamage = 0f;
    private Vector3 direction = Vector3.zero;
    private float cooldownTimer = 0f;
    private float immortalityTimer = 0f;
    private Vector3 startPos = Vector3.zero;
    private float startHealth = 0;
    private MeshRenderer playerMeshRenderer = null;
    private IWeapon currentWeapon = null;
    private int currentWeaponNumber = 0;
    private bool isImmortal = false;
    
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
        currentWeapon = weaponList[0].GetComponent<IWeapon>();
        shieldParticleSystem = playerShield.GetComponent<ParticleSystem>();
        weaponsSpecificFireRate = fireRate;
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

        Shield();
        SwapWeapon();

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

    private void SwapWeapon()
    {
       
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentWeaponNumber--;
            currentWeaponNumber = currentWeaponNumber < 0 ? weaponList.Count - 1 : currentWeaponNumber;

        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            currentWeaponNumber++;
            currentWeaponNumber = currentWeaponNumber > weaponList.Count - 1 ? 0 : currentWeaponNumber;
        }

        ActivateWeapon(currentWeaponNumber);
    }

    private void ActivateWeapon(int weapon)
    {
        switch (weapon)
        {
            case 0:
                weaponsSpecificFireRate = currentFireRate;
                break;
            case 1:
                weaponsSpecificFireRate = currentFireRate * 3.0f;
                break;
            case 2:
                weaponsSpecificFireRate = currentFireRate * 5.0f;
                break;

        }
        currentWeapon = weaponList[weapon].GetComponent<IWeapon>();
    }
    

    private void Fire()
    {
        if(cooldownTimer >= weaponsSpecificFireRate)
        {
            currentWeapon.Shoot(currentDamage);

            cooldownTimer = 0;
            DroneFire();
        }
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

            switch (powerEnum)
            {
                case PowerUpEnums.PowerEnum.SPREAD:
                    weaponList[0].GetComponent<IWeapon>().Upgrade();
                    ;
                    break;
                case PowerUpEnums.PowerEnum.MISSILE:
                    weaponList[1].GetComponent<IWeapon>().Upgrade();
                    weaponList[2].GetComponent<IWeapon>().Upgrade();
                    break;
                default:
                    break;
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

    public void Shield()
    {
        if(Input.GetKeyDown(KeyCode.R) && Mathf.Approximately(shieldTimer, 0.0f))
        {
            StartCoroutine(ShieldTimer());
        }
    }

    public override void TakeDamage(float dmg)
    {
        if ( isImmortal == true || immortalityTimer <= 2f)
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
        ActivateWeapon(0);

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
        float gameTimer = GameVariables.GameTime;
        while (immortalityTimer < 1.9f)
        {
            playerMeshRenderer.enabled = false;
            yield return new WaitForSeconds(gameTimer * 0.1f);
            playerMeshRenderer.enabled = true;
            yield return new WaitForSeconds(gameTimer * 0.1f);
        }
        yield return null;
    }

    private IEnumerator ShieldTimer()
    {
        float gameTimer = GameVariables.GameTime;

        shieldParticleSystem.Play();
        isImmortal = true;
        while(shieldTimer <= shieldDuration)
        {
            shieldTimer += gameTimer;
            yield return null;
        }

        shieldParticleSystem.Stop();
        isImmortal = false;
        while(shieldTimer <= shieldCooldown)
        {
            shieldTimer += gameTimer;
            yield return null;
        }
        shieldTimer = 0.0f;

    }

    public float GetShieldDuration()
    {
        return shieldDuration;
    }

}



