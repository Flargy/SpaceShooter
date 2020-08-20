using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupBase : MonoBehaviour
{
    [SerializeField] private float movespeed = 1;

    [SerializeField] private PowerUpEnums.PowerEnum powerUpType;

    private CapsuleCollider myCollider; 
    RaycastHit hit;
    // Start is called before the first frame update
    void Awake()
    {
        powerUpType = (PowerUpEnums.PowerEnum)Random.Range(0, 5);
        Debug.Log("Power up is " + powerUpType.ToString());
        myCollider = GetComponent<CapsuleCollider>();
        GetComponent<Renderer>().sharedMaterial = GameVariables.PowerUpMaterials[(int)powerUpType];
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    protected virtual void Move()
    {
        
        transform.position += transform.forward * movespeed * GameVariables.GameTime;
        
    }

    //protected virtual bool CheckCollision()
    //{
    //    Physics.Raycast(transform.position, transform.forward, out hit, (movespeed * GameVariables.GameTime) + myCollider.radius);
    //    return hit.collider == null ? true : false;
    //}

    private void PowerUpPlayer()
    {
        //PlayerBehaviour collision = hit.collider.gameObject.GetComponent<PlayerBehaviour>();

        //if(collision != null && GameVariables.Player == collision)
        //{
            GameVariables.Player.PowerUp(powerUpType);
            Destroy(gameObject);
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        PowerUpPlayer();
    }
}

 public class PowerUpEnums : MonoBehaviour
{
    public enum PowerEnum 
    { 
        SPREAD = 0, 
        MISSILE = 1, 
        DRONE = 2, 
        DAMAGE = 3, 
        FIRERATE = 4 
    };
}
