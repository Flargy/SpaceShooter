using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBase : DamageableObject
{
    [SerializeField] protected float movementSpeed = 5;
    [SerializeField] protected List<Transform> firePoints = new List<Transform>();
    [SerializeField] protected GameObject Lazerbeam = null;
    [SerializeField] protected GameObject mines = null;

    [SerializeField] protected float primaryFireRate = 0.5f;
    [SerializeField] protected float secondaryFireRate = 8;

    [SerializeField] private List<ParticleSystem> explosions = new List<ParticleSystem>();

    protected float primaryTimer = 0;
    protected float secondaryTimer = 0;
    protected bool immune = true;
    protected bool combatActivated = false;
    protected bool defeated = false;
    protected List<GameObject> listOfMines = new List<GameObject>();
    protected RaycastHit hit;
    protected float explosiontimer = 0;
    // Update is called once per frame
    protected override void Update()
    {
        if (!immune)
        {
            if (!defeated)
            {
                primaryTimer += GameVariables.GameTime;
                secondaryTimer += GameVariables.GameTime;
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
            else
            {
                transform.position += transform.forward * movementSpeed * GameVariables.GameTime;
                explosiontimer += GameVariables.GameTime;
                if (explosiontimer >= 0.25f)
                {
                    StartExplosions();
                    explosiontimer = 0;
                }

            }
        }
        else
        {
            transform.position += transform.forward * movementSpeed * GameVariables.GameTime;

            if(transform.position.z <= 10)
            {
                immune = false;
            }
        }
    }

    public void KillThisObject(GameObject go)
    {
        if (listOfMines.Contains(go))
        {
            listOfMines.Remove(go);
            Destroy(go);
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
        mine1.GetComponent<Mines>().parentBoss = this;
        listOfMines.Add(mine1);
        GameObject mine2 = Instantiate(mines, firePoints[4].position, firePoints[4].rotation);
        listOfMines.Add(mine2);
        mine2.GetComponent<Mines>().parentBoss = this;
    }

    public override void TakeDamage(float dmg)
    {
        if (!immune)
        {
            health -= dmg;
            Debug.Log("Boss Has " + health + " left and my imune status was " + immune);
            if (health <= 0)
            {
                List<GameObject> deadMines = listOfMines;
                foreach(GameObject obj in deadMines)
                {
                    KillThisObject(obj);
                }
                deadMines.Clear();

                OnDefeat();
                EnemySpawner.Instance.BossDefeated();
                //Destroy(gameObject);
            }
        }
    }

    private void StartExplosions()
    {
        int index = Random.Range(0, explosions.Count - 1);
        if (explosions[index].isPlaying)
        {
            StartExplosions();
        }
        else
        {
            explosions[index].Play();
        }
    }

    protected virtual void OnDefeat()
    {
        defeated = true;
        immune = true;
        GetComponent<Collider>().enabled = false;
        EnemySpawner.Instance.RemoveEnemy();
        Destroy(gameObject, 7.5f);
    }
}
