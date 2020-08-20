using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBase : MonoBehaviour
{
    [SerializeField] protected float health;
    [SerializeField] protected float movementSpeed = 5;


    [SerializeField] protected List<Transform> firePoints;
    [SerializeField] protected GameObject Lazerbeam;
    [SerializeField] protected GameObject mines;

    [SerializeField] protected float primaryFireRate = 0.5f;
    [SerializeField] protected float secondaryFireRate = 8;

    protected float primaryTimer = 0;
    protected float secondaryTimer = 0;
    protected bool immune = false;
    protected bool combatActivated = false;
    protected bool defeated = false;

    protected RaycastHit hit;

    // Start is called before the first frame update
    protected void Start()
    {
        
    }

    // Update is called once per frame
    protected void Update()
    {
        if (!immune)
        {
            primaryTimer += GameVariables.GameTime;
            secondaryTimer += GameVariables.GameTime;
            if (!defeated)
            {
                if(primaryTimer >= primaryFireRate)
                {
                    Fire();
                    primaryTimer = 0;
                }
                if(secondaryTimer >= secondaryFireRate)
                {
                    SecondaryFire();
                    secondaryTimer = 0;
                }
            }
        }
    }


    protected void Fire()
    {
        int index = Random.Range(0,2);
        GameObject pew = Instantiate(Lazerbeam, firePoints[index].position, firePoints[index].rotation);
    }

    protected void SecondaryFire()
    {
        GameObject mine1 = Instantiate(mines, firePoints[3].position, firePoints[3].rotation);
        GameObject mine2 = Instantiate(mines, firePoints[4].position, firePoints[4].rotation);
    }

    public void TakeDamage(float dmg)
    {
        if (!immune)
        {
            health -= dmg;
            if (health <= 0)
            {
                EnemySpawner.Instance.BossDefeated();
                Destroy(gameObject);
            }
        }
    }

    protected virtual void OnDefeat()
    {
        defeated = true;
        immune = true;
        GetComponent<Collider>().enabled = false;
    }
}
