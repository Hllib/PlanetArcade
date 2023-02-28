using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private int _damage = 1;
    [SerializeField]
    private float _destroyTime = 10.0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable hit = collision.GetComponent<IDamageable>();

        if (hit != null)
        {
            StartCoroutine(DealDamage(hit));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IDamageable hit = collision.GetComponent<IDamageable>();

        if (hit != null)
        {
            StopAllCoroutines();
        }
    }

    private void Start()
    {
        Destroy(gameObject, _destroyTime);
    }

    IEnumerator DealDamage(IDamageable hit)
    {
        while (true)
        {
            hit.Damage(_damage);
            yield return new WaitForSeconds(1.5f);
        }
    }
}
