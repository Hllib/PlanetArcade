using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour, IDamageable
{
    public int Health { get; set; }

    public void Damage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            ActivateFire();
        }
    }

    void Start()
    {
        Health = 1;
    }

    private void ActivateFire()
    {
        foreach(Fire fire in this.transform.GetComponentsInChildren<Fire>(true))
        {
            fire.gameObject.SetActive(true);
        }
    }
}
