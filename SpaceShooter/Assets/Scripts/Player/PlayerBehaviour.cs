using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;

    private int baseDamage = 1;
    private float currentDamage = 0f;
    private Vector3 direction = Vector3.zero;
    private float currentFireRate = 0f;
    private int fireRate = 1;
    private float cooldownTimer = 0f;
    


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
        Move();
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

    
}
