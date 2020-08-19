using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMissile : MissileBase
{
    public Transform Target { get; set; }

    protected override void Awake()
    {
        base.Awake();
        Target = GameVariables.GetEnemy();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if(Target != null)
        {
            transform.LookAt(Target);
        }
        base.Update();
    }
}
