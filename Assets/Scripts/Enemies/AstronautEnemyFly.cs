using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstronautEnemyFly : Enemy, IDamageable
{
    public int Health { get; set; }
    private int _initialHealth;

    private float _canAttack = 0.0f;
    private float _attackRate = 2.0f;
    private float _attackRadius = 6.5f;
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

        CheckAttackZone(_attackRadius);
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

    private void CheckAttackZone(float attackRadius)
    {
        float distance = Vector3.Distance(this.transform.localPosition, player.transform.localPosition);

        if (distance < attackRadius && !GameManager.Instance.IsPlayerDead)
        {
            _isAlerted = true;
            if (Time.time > _canAttack)
            {
                Fire();
            }
            isInCombat = true;
        }
        if (distance > attackRadius && _isAlerted)
        {
            isInCombat = false;
            _isAlerted = false;
        }
    }

    private void Fire()
    {
        _canAttack = Time.time + _attackRate;

        //attack
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.fire, this.transform.position);
    }
}
