using UnityEngine;

public class EnemyLazer : EnemySerpentine
{
    [SerializeField] private float chargeTime;
    [SerializeField] private float duration;

    private bool charging;
    private bool chaneling;

    // Update is called once per frame
    protected override void Update()
    {
        Physics.Raycast(transform.position, transform.forward, out hit, 100f);
        if(hit.collider != null && hit.collider.GetComponent<PlayerBehaviour>() == GameVariables.Player)
        {
            Fire();
        }
        else
        {
            base.Movmentbehaviour();
        }
    }

    protected override void Fire()
    {
        base.Fire();
    }

    public override void TakeDamage(float dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            EnemySpawner.Instance.RemoveEnemy();
            Destroy(gameObject);
        }
    }
}
