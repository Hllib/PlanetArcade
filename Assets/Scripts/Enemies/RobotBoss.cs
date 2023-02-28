using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBoss : Enemy, IDamageable
{
    public int Health { get; set; }
    private int _initialHealth;

    private float _canAttack = 0.0f;
    private float _attackRate = 3.0f;
    private float _attackRadius = 10.0f;

    private bool _powerAttackEnabled;
    private int _abilityCountdown;

    [SerializeField]
    private GameObject _firePillarPrefab;
    [SerializeField]
    private GameObject _fireHandPrefab;
    [SerializeField]
    private GameObject _shield;

    private bool _isShieldActive;

    private RobotBossAnimator _animator;

    public void Damage(int damage)
    {
        if (isDead) return;

        if (_isShieldActive)
        {
            OnShield(damage);
        }
        else
        {
            OnDamage(damage);
        }

        if (Health <= 0)
        {
            OnDeath();
        }
    }

    private void OnDamage(int damage)
    {
        Health -= damage;
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.hit, this.transform.position);
        UpdateHealthBar(Health * 100 / _initialHealth);
        ShowFloatingDamage(damage, Color.red);
        _animator.OnDamage();
        _powerAttackEnabled = true;
    }

    private void OnDeath()
    {
        _animator.OnDeath();

        if (lootPrefab != null)
        {
            DropLoot();
        }

        isDead = true;
        Destroy(gameObject, 1f);
    }

    private void OnShield(int damage)
    {
        if (Health < _initialHealth)
        {
            Health += damage;
            UpdateHealthBar(Health * 100 / _initialHealth);
        }
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.shieldDeflect, this.transform.position);
        ShowFloatingDamage(damage, Color.green);
    }

    public override void Init()
    {
        base.Init();

        health = 200;
        Health = health;
        _initialHealth = Health;
        speed = 0;

        _animator = GetComponent<RobotBossAnimator>();
    }

    public override void CalculateMovement()
    {
        CheckInCombatDirection();
        CheckAttackZone(_attackRadius);
    }

    private void CheckAttackZone(float attackRadius)
    {
        float distance = Vector3.Distance(this.transform.localPosition, player.transform.localPosition);

        if (distance < attackRadius)
        {
            if (Time.time > _canAttack)
            {
                Attack();
                _canAttack += _attackRate;
            }
            isInCombat = true;
        }
        else if (distance >= attackRadius)
        {
            isInCombat = false;
        }
    }

    private void Attack()
    {
        if (_abilityCountdown >= 10)
        {
            StartCoroutine(AbilityActivation());
            _abilityCountdown = 0;
        }
        else
        {
            if (_powerAttackEnabled)
            {
                PowerAttack();
                _powerAttackEnabled = false;
            }
            else
            {
                int attackOption = Random.Range(0, 4);

                switch (attackOption)
                {
                    case 0: FireHandAttack(); _abilityCountdown++; break;
                    default: FirePillarAttack(); _abilityCountdown++; break;
                }
            }
        }
    }

    private void FireHandAttack()
    {
        int offsetX = Random.Range(1, 5);
        int offsetY = Random.Range(-1, 4);

        GameObject instance = Instantiate(_fireHandPrefab, this.transform, false);
        instance.transform.position = new Vector3(transform.position.x + offsetX,
            transform.position.y + offsetY, transform.position.z);

        _animator.FireHandAttack();
    }

    private void FirePillarAttack()
    {
        int offsetX = 3;

        GameObject instance = Instantiate(_firePillarPrefab, this.transform, false);
        instance.transform.position = new Vector3(transform.position.x + offsetX,
            transform.position.y, transform.position.z);

        _animator.FirePillarAttack();
    }

    private void PowerAttack()
    {
        int fireHandAmount = 10;

        for (int i = 0; i < fireHandAmount; i++)
        {
            int offsetX = Random.Range(1, 7);
            int offsetY = Random.Range(-1, 6);

            GameObject instance = Instantiate(_fireHandPrefab, this.transform, false);
            instance.transform.position = new Vector3(transform.position.x + offsetX,
                transform.position.y + offsetY, transform.position.z);
        }

        _animator.FireHandAttack();
    }

    IEnumerator AbilityActivation()
    {
        _animator.Ability();
        _shield.SetActive(true);
        _isShieldActive = true;

        yield return new WaitForSeconds(5f);

        _animator.Ability();
        _shield.SetActive(false);
        _isShieldActive = false;
    }

    public override void CheckInCombatDirection()
    {

    }
}
