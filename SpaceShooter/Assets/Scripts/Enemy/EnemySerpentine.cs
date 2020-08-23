using UnityEngine;

public class EnemySerpentine : EnemyBase
{
    [SerializeField] protected float horizontalMovespeed = 5;
    [SerializeField] protected float HorizontalDistace = 5;

    float startPos = 0;
    Vector3 currentDirection = Vector3.zero;

    protected override void Start()
    {
        base.Start();

        if(Random.Range(0,1) > 0)
        {
            currentDirection.x = 1;
        }
        else
        {
            currentDirection.x = -1;
        }
        startPos = transform.position.x;
    }


    protected override void Update()
    {
        Movmentbehaviour();
    }

    protected override void Movmentbehaviour()
    {
        float distanceForward = movementSpeed * GameVariables.GameTime;
        Vector3 direction = transform.position + (transform.forward.normalized * distanceForward);
        direction += (currentDirection * horizontalMovespeed * GameVariables.GameTime);
        Debug.DrawLine(transform.position, direction, Color.red);
        direction -= transform.position;
        if (CheckCollision(direction, direction.magnitude)){

            transform.position += currentDirection.normalized * horizontalMovespeed * GameVariables.GameTime;
            transform.position += transform.forward.normalized * movementSpeed * GameVariables.GameTime;
            if (startPos + HorizontalDistace < transform.position.x || startPos - HorizontalDistace > transform.position.x)
            {
                currentDirection.x *= -1;
            }
        }
    }
}
