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
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform projectileSpawn;
    [SerializeField] private List<Transform> SpreadSpawPoints = new List<Transform>();
    [SerializeField] private List<Transform> missileSpawnpoints = new List<Transform>();

    [SerializeField] private bool weaponType = false; ///yolo false == spread, true == missile :D

    private Dictionary<bool, int> upgrades = new Dictionary<bool, int>();


    private float missileCoolDown = 0;

    private float currentDamage = 0f;
    private Vector3 direction = Vector3.zero;
    private int fireRate = 1;
    private float cooldownTimer = 0f;
    private float immortalityTimer = 0f;
    


    void Awake()
    {
        upgrades.Add(false, 0);
        upgrades.Add(true, 0);
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
            upgrades[false]++;
            Debug.Log("Vale upgrae to :" + upgrades[false]);
        }
        Move();

        immortalityTimer += immortalityTimer < 1.5f ? GameVariables.GameTime : 0f;
    }

    private void Fire()
    {
        //cooldownTimer >= currentFireRate ? Debug.Log("hej") : Debug.Log("inte hej");
        if(cooldownTimer >= currentFireRate)
        {
            //Debug.Log("fire");
            GameObject bullet = Instantiate(projectile, projectileSpawn.position, projectileSpawn.rotation);
            int index = 0;
            for(int i = 1; i <= upgrades[false]; i++)
            {
                GameObject bullet1 = Instantiate(projectile, SpreadSpawPoints[index].position, SpreadSpawPoints[index].rotation);
                index++;
                GameObject bullet2 = Instantiate(projectile, SpreadSpawPoints[index].position, SpreadSpawPoints[index].rotation);
                index++;

                if (index >= 7)
                    break;
            }

            index = 0;


            cooldownTimer = 0;
        }
        else
        {
            //Debug.Log("not fire");
        }
    }

    private void Move()
    {

        transform.position += direction * movementSpeed * GameVariables.GameTime;

    }

    private void UseAbility()
    {

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
