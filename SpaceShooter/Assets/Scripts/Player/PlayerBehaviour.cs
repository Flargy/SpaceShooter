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

    private float currentDamage = 0f;
    private Vector3 direction = Vector3.zero;
    private int fireRate = 1;
    private float cooldownTimer = 0f;
    private float immortalityTimer = 0f;
    


    void Awake()
    {
        currentDamage = baseDamage;
        currentFireRate = fireRate;
    }

    void Update()
    {
        direction.x = Input.GetAxis("Horizontal");
        direction.y = Input.GetAxis("Vertical");
        cooldownTimer += GameVariables.GameTime;
        if (Input.GetKey(KeyCode.Space))
        {
            Fire();
        }

        if (Input.GetKey(KeyCode.P))
        {
            ReceiveDamage();
        }
        Move();

        immortalityTimer += immortalityTimer < 1.5f ? GameVariables.GameTime : 0f;


        GameVariables.PlayerTransform = transform;
    }

    private void Fire()
    {
        //cooldownTimer >= currentFireRate ? Debug.Log("hej") : Debug.Log("inte hej");
        if(cooldownTimer >= currentFireRate)
        {
            Debug.Log("fire");
            cooldownTimer = 0;
        }
        else
        {
            Debug.Log("not fire");
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
