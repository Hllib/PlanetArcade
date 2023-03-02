using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodWall : MonoBehaviour, IDamageable
{
    public int Health { get; set; }
    private Animator _anim;

    public void Damage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            _anim.SetTrigger("Destroy");
            Destroy(gameObject, 1.5f);
        }
    }

    void Start()
    {
        Health = 1;
        _anim = GetComponent<Animator>();
    }
}
