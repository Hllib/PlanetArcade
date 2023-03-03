using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cactus : MonoBehaviour
{
    private int _damage = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable hit = collision.GetComponent<IDamageable>();

        if (hit != null)
        {
            hit.Damage(_damage);

            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.cactusHit, Vector3.zero);
        }
    }
}
