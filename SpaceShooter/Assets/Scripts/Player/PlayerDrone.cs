using UnityEngine;

public class PlayerDrone : DamageableObject
{
    [SerializeField] private Transform player = default;
    [SerializeField] private Collider myCollider = null;
    [SerializeField] private Renderer myRenderer = null;
    [SerializeField] private Transform firePoint = default;


    public bool Active = false;


    // Start is called before the first frame update
    protected override void Start()
    {
        myCollider.enabled = false;
        myRenderer.enabled = false;
        Active = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (Active)
        {
            transform.LookAt(transform.position + player.transform.forward);
        }   
    }


    public override void TakeDamage(float damage)
    {
        health -= 1;
        if(health <= 0)
        {
            ActivateDrone(false);
        }
    }

    public void ActivateDrone(bool status)
    {
        myCollider.enabled = status;
        myRenderer.enabled = status;
        Active = status;
    }

    public void Fire(float damage)
    {
        if (Active)
        {
            GameObject bullet = ObjectPool.Instance.GetPooledLazer();
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = firePoint.rotation;
            bullet.SetActive(true);
            bullet.GetComponent<ProjectileBase>().Damage = damage;
        }
    }
}
