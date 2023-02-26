using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstronautEnemyFly : Enemy, IDamageable
{
    public int Health { get; set; }
    private int _initialHealth;

    private float _canAttack = 0.0f;
    private float _attackRate = 2.0f;
    private float _attackRadius = 5.0f;
    private bool _isAlerted;

    [SerializeField]
    private GameObject _smallBulletPrefab;
    [SerializeField]
    private GameObject _bigBulletPrefab;

    private int _shotCounter;

    enum HorizontalLookDirection
    {
        Left,
        Right
    }
    public int lookDirection;

    public override void Init()
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
        if (GameManager.Instance.IsPlayerDead) return;

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
        CheckInCombatDirection();
    }

    public void Damage(int damage)
    {
        if (isDead) return;

        Health -= damage;
        UpdateHealthBar(Health * 100 / _initialHealth);
        ShowFloatingDamage(damage);
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

    public override void CheckInCombatDirection()
    {
        Vector3 direction = player.transform.localPosition - transform.localPosition;

        if (direction.x > 0 && isInCombat)
        {
            spriteRenderer.flipX = false;
            lookDirection = (int)HorizontalLookDirection.Right;
        }
        else if (direction.x < 0 && isInCombat)
        {
            spriteRenderer.flipX = true;
            lookDirection = (int)HorizontalLookDirection.Left;
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
            //currentTarget = previousTarget;
            _isAlerted = false;
        }
    }

    private void Fire()
    {
        _canAttack = Time.time + _attackRate;
        float bulletRotationY = 0f;
        switch (lookDirection)
        {
            case (int)HorizontalLookDirection.Left: bulletRotationY = 180; break;
            case (int)HorizontalLookDirection.Right: bulletRotationY = 0; break;
        }
        switch (_shotCounter)
        {
            case < 3: 
                Instantiate(_smallBulletPrefab, transform.position, Quaternion.Euler(0f, bulletRotationY, 0));
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.fire, this.transform.position);               
                _shotCounter++; 
                break;
            case >= 3: 
                Instantiate(_bigBulletPrefab, transform.position, Quaternion.Euler(0f, bulletRotationY, 0));
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.fireBig, this.transform.position);
                _shotCounter = 0; 
                break;
        }
    }
}
