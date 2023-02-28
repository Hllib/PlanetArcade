using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBoss : Enemy, IDamageable
{
    public int Health { get; set; }
    private int _initialHealth;

    public void Damage(int damage)
    {
        throw new System.NotImplementedException();
    }

    public override void Init()
    {
        base.Init();

        health = 200;
        Health = health;
        _initialHealth = Health;
        speed = 0;
    }
}
