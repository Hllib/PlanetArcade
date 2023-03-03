using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodWall : MonoBehaviour, IDamageable
{
    public int Health { get; set; }
    private Animator _anim;
    private bool _isDead;

    public void Damage(int damage)
    {
        if (_isDead) return;

        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.woodCrack, Vector3.zero);
        Health -= damage;

        if (Health <= 0)
        {
            _anim.SetTrigger("Destroy");
            Destroy(gameObject, 0.5f);
            _isDead = true;
        }
    }

    void Start()
    {
        Health = 1;
        _anim = GetComponent<Animator>();
    }
}
