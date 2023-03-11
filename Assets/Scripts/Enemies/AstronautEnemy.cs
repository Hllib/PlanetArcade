using FMOD;
using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstronautEnemy : Enemy, IDamageable
{
    public int Health { get; set; }
    private int _initialHealth;

    private float _canAttack = 0.0f;
    private float _attackRate = 2.0f;
    private float _attackRadius = 6.0f;
    private bool _isAlerted;

    protected override void Init()
    {
        base.Init();

        speed = 3;
        health = 50;
        Health = base.health;
        _initialHealth = Health;
        tempSpeed = speed;
    }

    protected override void Update()
    {
        base.Update();
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
            speed = 0;
            StopAllCoroutines();
            animator.SetBool("Walk", false);
            if (Time.time > _canAttack)
            {
                Fire();
            }
            isInCombat = true;
        }
        if (distance > attackRadius && _isAlerted)
        {
            isInCombat = false;
            speed = tempSpeed;
            animator.SetBool("Walk", true);
            currentTarget = previousTarget;
            _isAlerted = false;
        }
    }

    private void Fire()
    {
        _canAttack = Time.time + _attackRate;

        //attack
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.fire, this.transform.position);
    }

    public void StepSound()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.enemyFootstep, this.transform.position);
    }
}
