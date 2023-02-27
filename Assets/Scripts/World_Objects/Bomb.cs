using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour, IDamageable
{
    public int Health { get; set; }
    [SerializeField]
    private Animator _anim;

    public void Damage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            _anim.SetTrigger("Explode");
            DealDamageAround(50);
            Destroy(gameObject, 0.5f);
        }
    }

    void Start()
    {
        _anim = GetComponent<Animator>();
        Health = 1;
    }

    private void DealDamageAround(int damage)
    {

    }
}
