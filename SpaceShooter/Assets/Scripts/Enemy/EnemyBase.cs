using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{

    [SerializeField] protected float health = 10;

    [SerializeField] protected float movementSpeed = 5;

    [SerializeField] protected GameObject projectile;
    [SerializeField] protected Transform projectileFire;

    [SerializeField] private float fireRate = 5;
    [SerializeField] private bool isBoss = false;

    private float coolDownTimer = 0;

    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        float modifier = EnemySpawner.Instance.DifficultyMultiplier;
        health = health * modifier;
        fireRate = fireRate - 1 * 0.2f * modifier;
    }

    // Update is called once per frame
    void Update()
    {
        Movmentbehaviour();
        coolDownTimer += GameVariables.GameTime;
        if(coolDownTimer > fireRate)
        {
            Fire();
            coolDownTimer = 0;
        }
    }

    public void TakeDamage(float dmg)
    {
        Debug.Log("Ow");
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

    private void Fire()
    {
        GameObject pew = Instantiate(projectile, projectileFire.position, projectileFire.rotation);
    }

    protected virtual void Movmentbehaviour()
    {
        //if (CheckCollision())
        //{
            transform.position += transform.forward.normalized * movementSpeed * GameVariables.GameTime;
        //}
    } 

    protected virtual bool CheckCollision()
    {
        Physics.Raycast(transform.position, transform.forward, out hit, movementSpeed * GameVariables.GameTime);
        return hit.collider == null ? true : false;
    }

    private void OnDisable()
    {
        if(isBoss == false)
        {
            EnemySpawner.Instance.EnemyOutOfBounds();
        }
    }
}
