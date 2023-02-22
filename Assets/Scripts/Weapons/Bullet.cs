using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float _speed = 7.0f;
    private bool _hasDamaged;
    [SerializeField]
    private int _damage;

    private void Start()
    {
        Destroy(gameObject, 1.0f);
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(new Vector3(_speed * Time.deltaTime, 0, 0));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable hit = other.GetComponent<IDamageable>();

        if (hit != null && !_hasDamaged)
        {
            hit.Damage(_damage);
            _hasDamaged = true;
        }
    }
}
