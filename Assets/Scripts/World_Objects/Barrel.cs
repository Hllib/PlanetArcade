using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour, IDamageable
{
    public int Health { get; set; }
    private Animator _barrelAnim;

    public void Damage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.fireHandLand, this.transform.position);
            _barrelAnim.SetTrigger("Fire");
            ActivateFire();
            Destroy(gameObject, 10.0f);
        }
    }

    void Start()
    {
        Health = 1;
        _barrelAnim = GetComponent<Animator>();
    }

    private void ActivateFire()
    {
        foreach(Fire fire in this.transform.GetComponentsInChildren<Fire>(true))
        {
            fire.gameObject.SetActive(true);
        }
    }
}
