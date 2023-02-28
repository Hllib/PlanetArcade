using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class Slime : Enemy, IDamageable
{
    public int Health { get; set; }
    private int _initialHealth;

    private float _canAttack = 0.0f;
    private float _attackRate = 2.0f;
    private float _chaseStartRadius = 3.0f;
    private float _chaseStopRadius = 5.0f;
    private float _attackRadius = 1.2f;

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
            UIManager.Instance.DisplayMessage("Slime destroyed!");
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.slimeDeath, this.transform.position);
        }
    }

    public override void Init()
    {
        base.Init();

        speed = 2;
        health = 20;
        Health = this.health;
        _initialHealth = Health;
        tempSpeed = speed;
    }

    private void CheckAttackZone(float chaseStartTarget, float chaseStopRadius, float attackRadius)
    {
        float distance = Vector3.Distance(this.transform.localPosition, player.transform.localPosition);
        if (distance < chaseStartTarget)
        {
            currentTarget = player.transform.position;
            isInCombat = true;
        }
        if (distance < attackRadius && Time.time > _canAttack && !GameManager.Instance.IsPlayerDead)
        {
            StartCoroutine(Attack());
            _canAttack = Time.time + _attackRate;
        }
        if (distance > chaseStopRadius)
        {
            isInCombat = false;
            currentTarget = previousTarget;
        }
    }

    IEnumerator Attack()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.slimeAttack, this.transform.position);    
        this.animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);
        player.Damage(1);
    }

    public override void CalculateMovement()
    {
        if (GameManager.Instance.IsPlayerDead) return;

        CheckAttackZone(_chaseStartRadius, _chaseStopRadius, _attackRadius);
        base.CalculateMovement();
    }

    public void JumpSound()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.slimeJump, this.transform.position);
    }
}
