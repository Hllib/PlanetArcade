using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField]
    private int _damage = 1;
    [SerializeField]
    private float _destroyTime = 10.0f;
    [SerializeField]
    private bool _isConst = false;

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
        if (!_isConst)
        {
            Destroy(gameObject, _destroyTime);
        }
    }

    IEnumerator DealDamage(IDamageable hit)
    {
        while (true)
        {
            hit.Damage(_damage);
            yield return new WaitForSeconds(1.5f);
        }
    }

    public void PlayExplosionSound()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.fireHandLand, this.transform.position);
    }
}
