using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBossAnimator : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void FirePillarAttack()
    {
        _animator.SetTrigger("Pillar");
    }

    public void FireHandAttack()
    {
        _animator.SetTrigger("Hand");
    }

    public void Ability()
    {
        _animator.SetTrigger("Ability");
    }

    public void OnDamage()
    {
        _animator.SetTrigger("Hit");
    }

    public void OnDeath()
    {
        _animator.SetTrigger("Death");
    }
}
