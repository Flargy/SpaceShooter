using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableObject : MonoBehaviour
{
    [SerializeField] protected float health = 10;
    // Start is called before the first frame update


    protected virtual void Awake() { }
    protected virtual void Start() { }
    protected virtual void Update() { }
    public virtual void TakeDamage(float dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            EnemySpawner.Instance.RemoveEnemy();
            Destroy(gameObject);
        }
    }

    public virtual void DestroyMyGameObject()
    {
        if(gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}
