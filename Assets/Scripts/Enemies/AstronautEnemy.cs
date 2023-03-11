using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AstronautEnemy : Enemy, IDamageable
{
    public int Health { get; set; }
    private int _initialHealth;

    [SerializeField]
    private Transform _gunPoint;

    protected override void Init()
    {
        base.Init();

        speed = 3;
        health = 50;
        Health = base.health;
        _initialHealth = Health;
        tempSpeed = speed;
    }

    protected override void SetAttackSettings()
    {
        attackRate = 2.0f;
        chaseStartRadius = 5.5f;
        chaseStopRadius = 8.0f;
        attackRadius = 6.5f;
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

    protected override void Attack()
    {
        StartCoroutine(Fire());
    }


    [SerializeField]
    private LineRenderer _line;

    IEnumerator Fire()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.fire, this.transform.position);

        _line.gameObject.SetActive(true);
        _line.SetPosition(0, _gunPoint.position);
        _line.SetPosition(1, player.transform.position);
        player.Damage(1);
        yield return new WaitForSeconds(0.5f);

        _line.gameObject.SetActive(false);
    }

    public void StepSound()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.enemyFootstep, this.transform.position);
    }
}
