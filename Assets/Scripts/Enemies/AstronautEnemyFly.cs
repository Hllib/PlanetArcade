using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstronautEnemyFly : Enemy, IDamageable
{
    public int Health { get; set; }
    private int _initialHealth;

    private bool _isAlerted;

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
        }
        else if (transform.position == pointB.position)
        {
            currentTarget = pointA.position;
        }
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);
    }

    //DIFFERS FROM BASE CLASS
    protected override void CheckAttackZone()
    {
        float distance = Vector3.Distance(this.transform.localPosition, player.transform.localPosition);

        if (distance < attackRadius && !GameManager.Instance.IsPlayerDead)
        {
            _isAlerted = true;
            if (Time.time > canAttack)
            {
                Attack();
            }
            isInCombat = true;
        }
        if (distance > attackRadius && _isAlerted)
        {
            isInCombat = false;
            _isAlerted = false;
        }
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
        attackRadius = 6.5f;
    }

    protected override void Attack()
    {
        Fire();
    }

    private void Fire()
    {
        canAttack = Time.time + attackRate;
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.fire, this.transform.position);
    }
}
