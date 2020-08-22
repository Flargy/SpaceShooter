using System.Collections.Generic;
using UnityEngine;

public class BossBase : DamageableObject
{
    [SerializeField] protected float movementSpeed = 5f;
    [SerializeField] protected List<Transform> firePoints = new List<Transform>();
    [SerializeField] protected GameObject Lazerbeam = null;
    [SerializeField] protected GameObject mines = null;

    [SerializeField] protected int scoreValue = 50;

    [SerializeField] protected float primaryFireRate = 0.5f;
    [SerializeField] protected float secondaryFireRate = 8f;
    [SerializeField] protected float destructionTime = 7.5f;

    [SerializeField] private List<ParticleSystem> explosions = new List<ParticleSystem>();

    protected float primaryTimer = 0f;
    protected float secondaryTimer = 0f;
    protected bool immune = true;
    protected bool combatActivated = false;
    protected bool defeated = false;
    public List<GameObject> listOfMines = new List<GameObject>();
    protected RaycastHit hit;
    protected float explosiontimer = 0f;
    protected float multiplier = 2f;

    protected override void Start()
    {
        float modifier = EnemySpawner.Instance.DifficultyMultiplier;

        health = health * modifier;
        primaryFireRate = primaryFireRate * (1 - 0.05f * modifier);


        GameVariables.Instance.RegisterEnemy(this);
    }

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
                MoveSideToSide();
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
            MoveForward();
        }
    }


    private void MoveForward()
    {
        transform.position += transform.forward * movementSpeed * GameVariables.GameTime;

        if (transform.position.z <= 10)
        {
            immune = false;
            GameVariables.GameUI.AssignBossHealth(health);
        }
    }

    private void MoveSideToSide()
    {
        transform.position += transform.right * movementSpeed * GameVariables.GameTime * multiplier;
        if (transform.position.x >= 5)
        {
            multiplier = -multiplier;
        }
        else if (transform.position.x <= -5)
        {
            multiplier = -multiplier;
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
            GameVariables.GameUI.UpdateBossSlider(dmg);

            if (health <= 0)
            {
                foreach(GameObject obj in listOfMines)
                {
                    Destroy(obj);
                }
                listOfMines.Clear();

                GameVariables.GameUI.UpdatePlayerScore(scoreValue);

                OnDefeat();
                EnemySpawner.Instance.BossDefeated(destructionTime + 5f);
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
        foreach (GameObject obj in listOfMines)
        {
            Destroy(obj);
        }
        listOfMines.Clear();
        defeated = true;
        immune = true;
        GetComponent<Collider>().enabled = false;
        EnemySpawner.Instance.RemoveEnemy();
        Destroy(gameObject, 7.5f);
    }

    public override void DestroyMyGameObject()
    {
        OnDefeat();
    }
}
