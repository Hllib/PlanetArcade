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

    private int _attackCount;

    [SerializeField]
    private GameObject _firePillarPrefab;
    [SerializeField]
    private GameObject _fireHandPrefab;

    public void Damage(int damage)
    {
        if (isDead) return;

        Health -= damage;
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.hit, this.transform.position);
        UpdateHealthBar(Health * 100 / _initialHealth);
        ShowFloatingDamage(damage);

        if (Health <= 0)
        {
            if (lootPrefab != null)
            {
                DropLoot();
            }

            isDead = true;
            Destroy(gameObject);
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.slimeDeath, this.transform.position);
        }
    }

    public override void Init()
    {
        base.Init();

        health = 200;
        Health = health;
        _initialHealth = Health;
        speed = 0;
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
        if (_attackCount >= 5)
        {
            PowerAttack();
            _attackCount = 0;
        }
        else
        {
            int attackOption = Random.Range(0, 4);

            switch (attackOption)
            {
                case 0: FireHandAttack(); _attackCount++; break;
                default: FirePillarAttack(); _attackCount++; break;
            }
        }
    }

    private void FireHandAttack()
    {
        int offsetX = Random.Range(0, 4);
        int offsetY = Random.Range(0, 4);

        GameObject instance = Instantiate(_fireHandPrefab, this.transform, false);
        instance.transform.position = new Vector3(transform.position.x + offsetX, 
            transform.position.y + offsetY, transform.position.z);
    }

    private void FirePillarAttack()
    {
        int offsetX = 3;

        GameObject instance = Instantiate(_firePillarPrefab, this.transform, false);
        instance.transform.position = new Vector3(transform.position.x + offsetX,
            transform.position.y, transform.position.z);
    }

    private void PowerAttack()
    {
        int fireHandAmount = 3;

        for (int i = 0; i < fireHandAmount; i++)
        {
            int offsetX = Random.Range(0, 6);
            int offsetY = Random.Range(0, 6);

            GameObject instance = Instantiate(_fireHandPrefab, this.transform, false);
            instance.transform.position = new Vector3(transform.position.x + offsetX,
                transform.position.y + offsetY, transform.position.z);
            instance.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    public override void CheckInCombatDirection()
    {

    }
}
