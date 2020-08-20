using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mines : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2;
    [SerializeField] private Transform rotator;
    [SerializeField] private Transform[] firePoints;
    [SerializeField] private float fireRate = 1;
    [SerializeField] private GameObject projectile;

    private float fireTimer = 0;

    void Update()
    {
        fireTimer += GameVariables.GameTime;
        if(fireTimer >= fireRate)
        {
            Fire();
        }
        transform.position += transform.forward * moveSpeed * GameVariables.GameTime;

        rotator.RotateAround(transform.position, transform.up, 2f);
    }

    void Fire()
    {
        foreach(Transform trans in firePoints)
        {
            GameObject pew = Instantiate(projectile, trans.position, trans.rotation);
        }
        fireTimer = 0;
    }
}
