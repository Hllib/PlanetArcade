using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AstronautEnemy : Enemy, IDamageable
{
    public int Health { get; set; }
    protected int initialHealth;

    [SerializeField]
    protected Transform _gunPoint;
    [SerializeField]
    protected LineRenderer _line;

    protected override void SetInitialSettings()
    {
        EnemyScriptableObject AI = enemyScriptableObject;

        speed = AI.speed;
        tempSpeed = speed;
        Health = AI.health;
        initialHealth = Health;

        attackRadius = AI.attackRadius;
        attackRate = AI.attackRate;
        chaseStartRadius = AI.chaseStartRadius;
        chaseStopRadius = AI.chaseStopRadius;
    }

    public void Damage(int damage)
    {
        if (isDead) return;

        isInCombat = true;
        Health -= damage;
        UpdateHealthBar(Health * 100 / initialHealth);
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

    public IEnumerator Fire()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.fire, this.transform.position);

        Vector3 offset = new Vector3(0, 0.3f, 0);
        _line.gameObject.SetActive(true);
        _line.SetPosition(0, _gunPoint.position);
        _line.SetPosition(1, player.transform.position + offset);

        player.Damage(1);
        yield return new WaitForSeconds(0.5f);

        _line.gameObject.SetActive(false);
    }

    public void StepSound()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.enemyFootstep, this.transform.position);
    }
}
