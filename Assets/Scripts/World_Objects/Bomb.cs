using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour, IDamageable
{
    public int Health { get; set; }
    private Animator _anim;
    private CircleCollider2D _circleCollider;
    private bool _canExplode;
    private int _explosionDamage = 50;
    private bool _hasDamaged;

    public void Damage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.bomb, this.transform.position);
            _anim.SetTrigger("Explode");
            DealDamageAround();
            Destroy(gameObject, 0.5f);
        }
    }

    void Start()
    {
        _anim = GetComponent<Animator>();
        _circleCollider = GetComponent<CircleCollider2D>();

        Health = 1;
    }

    private void DealDamageAround()
    {
        _canExplode = true;
        _circleCollider.radius *= 4;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_canExplode)
        {
            IDamageable hit = collision.GetComponent<IDamageable>();

            if (hit != null && !_hasDamaged)
            {
                hit.Damage(_explosionDamage);
                _hasDamaged = true;
            }
        }
    }
}
