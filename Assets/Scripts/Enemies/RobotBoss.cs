using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBoss : Enemy, IDamageable
{
    public int Health { get; set; }
    private int _initialHealth;

    private float _closeCombatRadius = 5.0f;

    private bool _powerAttackEnabled;
    private int _abilityCountdown;

    [SerializeField]
    private GameObject _firePillarPrefab;
    [SerializeField]
    private GameObject _fireHandPrefab;
    [SerializeField]
    private GameObject _meteorPrefab;
    [SerializeField]
    private GameObject _shield;
    [SerializeField]
    private GameObject[] _fireRings;
    [SerializeField]
    private GameObject _portalToFinal;

    private bool _isShieldActive;

    private RobotBossAnimator _animator;

    protected override void SetInitialSettings()
    {
        EnemyScriptableObject AI = enemyScriptableObject;

        speed = AI.speed;
        Health = AI.health;
        _initialHealth = Health;

        attackRadius = AI.attackRadius;
        attackRate = AI.attackRate;
        chaseStartRadius = AI.chaseStartRadius;
        chaseStopRadius = AI.chaseStopRadius;
    }

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
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.bossHit, this.transform.position);
        Health -= damage;
        UpdateHealthBar(Health * 100 / _initialHealth);
        ShowFloatingDamage(damage, Color.red);
        _animator.OnDamage();
        _powerAttackEnabled = true;
    }

    private void OnDeath()
    {
        _animator.OnDeath();

        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.bossDeath, this.transform.position);
        MarsManager.Instance.StopFightMusic();
        _portalToFinal.SetActive(true);

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

    protected override void Init()
    {
        base.Init();
        _animator = GetComponent<RobotBossAnimator>();
    }

    public override void CalculateMovement()
    {

    }

    protected override void Update()
    {
        CheckLookDirection();
        CheckAttackZone();
        CheckCloseCombat();
    }

    private void CheckCloseCombat()
    {
        float distance = Vector3.Distance(this.transform.localPosition, player.transform.localPosition);

        if (distance < _closeCombatRadius)
        {
            foreach (var fireRing in _fireRings)
            {
                fireRing.SetActive(true);
            }
        }
        else
        {
            foreach (var fireRing in _fireRings)
            {
                fireRing.SetActive(false);
            }
        }
    }

    protected override void CheckAttackZone()
    {
        float distance = Vector3.Distance(this.transform.localPosition, player.transform.localPosition);

        if (distance < attackRadius)
        {
            if (Time.time > canAttack)
            {
                Attack();
                canAttack = Time.time + attackRate;
            }
            isInCombat = true;
        }
        else if (distance >= attackRadius)
        {
            isInCombat = false;
        }
    }

    protected override void Attack()
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
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.firePillar, this.transform.position);
    }

    private void FirePillarAttack()
    {
        int offsetX = 3;

        GameObject instance = Instantiate(_firePillarPrefab, this.transform, false);
        instance.transform.position = new Vector3(transform.position.x + offsetX,
            transform.position.y, transform.position.z);

        _animator.FirePillarAttack();
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.firePillar, this.transform.position);
    }

    private void PowerAttack()
    {
        _animator.FireHandAttack();
        StartCoroutine(SpawnMeteor());
    }

    IEnumerator SpawnMeteor()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.meteor, this.transform.position);
        int fireHandAmount = 5;
        for (int i = 0; i < fireHandAmount; i++)
        {
            int offsetX = Random.Range(1, 7);
            int offsetY = Random.Range(3, 7);

            GameObject instance = Instantiate(_meteorPrefab, this.transform, false);
            instance.transform.position = new Vector3(transform.position.x + offsetX,
                transform.position.y + offsetY, transform.position.z);
            yield return new WaitForSeconds(0.3f);
        }
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

    public override void CheckLookDirection()
    {

    }
}
