using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 3.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable hit = collision.gameObject.GetComponent<IDamageable>();
        if (hit != null) 
        {
            hit.Damage(5);
            Destroy(gameObject);
        }
    }
}
