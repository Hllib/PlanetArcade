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

    [SerializeField]
    public bool isTutorialSlime;

    protected override void SetAttackSettings()
    {
        attackRate = 2.0f;
        chaseStartRadius = 3.5f;
        chaseStopRadius = 5.0f;
        attackRadius = 1.2f;
    }

    public void Damage(int damage)
    {
        if (isDead) return;

        Health -= damage;
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.hit, this.transform.position);
        UpdateHealthBar(Health * 100 / _initialHealth);
        ShowFloatingDamage(damage, Color.red);

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

            if (isTutorialSlime)
            {
                Guide.Instance.HasFinishedTutorial = true;
                Guide.Instance.FinalMessage();
            }
        }
    }

    protected override void Init()
    {
        base.Init();

        speed = 2;
        health = 20;
        Health = this.health;
        _initialHealth = Health;
        tempSpeed = speed;
    }

    protected override void Attack()
    {
         StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.slimeAttack, this.transform.position);
        this.animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);
        player.Damage(1);
    }

    public void JumpSound()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.slimeJump, this.transform.position);
    }
}
