using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Timeline;

public class SlimeTutorial : Enemy, IDamageable
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
        UIManager.Instance.DisplayMessage($"Damage done: {damage}");

        Health -= damage;
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.hit, this.transform.position);
        UpdateHealthBar(Health * 100 / _initialHealth);
        ShowFloatingDamage(damage, Color.red);

        if (Health <= 0)
        {
            isDead = true;
            Destroy(gameObject);
            UIManager.Instance.DisplayMessage("Slime destroyed!");
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.slimeDeath, this.transform.position);
            Guide.Instance.HasFinishedTutorial = true;
            Guide.Instance.FinalMessage();
        }
    }

    public override void Init()
    {
        base.Init();

        speed = 2;
        health = 20;
        Health = base.health;
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
