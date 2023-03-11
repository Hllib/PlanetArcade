using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstronautEnemyFly : Enemy, IDamageable
{
    public int Health { get; set; }
    private int _initialHealth;

    protected override void Init()
    {
        base.Init();

        speed = 0.5f;
        health = 30;
        Health = base.health;
        _initialHealth = Health;
        tempSpeed = speed;
    }
    
    //DIFFERS FROM BASE CLASS
    public override void CalculateMovement()
    {
        if (transform.position == pointA.position)
        {
            currentTarget = pointB.position;
            previousTarget = pointB.position;
        }
        else if (transform.position == pointB.position)
        {
            currentTarget = pointA.position;
            previousTarget = pointA.position;
        }
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);
    }
    
    public void Damage(int damage)
    {
        if (isDead) return;

        Health -= damage;
        UpdateHealthBar(Health * 100 / _initialHealth);
        ShowFloatingDamage(damage, Color.red);
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.hit, this.transform.position);

        if (Health <= 0)
        {
            if (lootPrefab != null)
            {
                DropLoot();
            }
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.enemyDeath, this.transform.position);
            isDead = true;
            Destroy(gameObject);
            UIManager.Instance.DisplayMessage("Enemy astronaut destroyed!");
        }
    }

    protected override void SetAttackSettings()
    {
        attackRate = 2.0f;
        chaseStartRadius = 3.5f;
        chaseStopRadius = 5.0f;
        attackRadius = 1.2f;
    }

    protected override void Attack()
    {
        Fire();
    }

    private void Fire()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.fire, this.transform.position);
    }
}
