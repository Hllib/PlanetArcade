using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private int _damage = 1;
    [SerializeField]
    private bool _isOneShot = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            IDamageable hit = collision.GetComponent<IDamageable>();

            if (hit != null)
            {
                StartCoroutine(DealDamage(hit));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StopAllCoroutines();
        }
    }

    private void Start()
    {
        if(_isOneShot)
        {
            Destroy(gameObject, 1f);
        }
    }

    IEnumerator DealDamage(IDamageable hit)
    {
        while(true)
        {
            hit.Damage(_damage);
            yield return new WaitForSeconds(1.5f);
        }
    }
}
