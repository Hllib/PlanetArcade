using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstronautEnemyFly : Enemy, IDamageable
{
    public int Health { get; set; }
    private int _initialHealth;

    [SerializeField]
    private Transform _gunPoint;
    [SerializeField]
    private LineRenderer _line;

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

    //DIFFERS FROM BASE CLASS
    public override void CalculateMovement()
    {
        if (transform.position == pointA.position)
        {
            currentTarget = pointB.position;
            previousTarget = pointB.position;
        }
        else if (transform.position == pointB.position)
        {
            currentTarget = pointA.position;
            previousTarget = pointA.position;
        }
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);
    }

    public void Damage(int damage)
    {
        if (isDead) return;

        isInCombat = true;
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

    IEnumerator Fire()
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
}
