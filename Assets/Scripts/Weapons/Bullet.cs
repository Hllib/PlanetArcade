using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int _damage;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void GetMessage(BulletScriptableObject bulletScriptableObject)
    {
        _damage = bulletScriptableObject.damage;
        _spriteRenderer.sprite = bulletScriptableObject.sprite;
    }

    private void OnEnable()
    {
        Invoke("Disable", 1.0f);
    }

    public void Disable()
    {
        this.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable hit = collision.gameObject.GetComponent<IDamageable>();
        if (hit != null)
        {
            hit.Damage(_damage);
            Disable();
        }
    }
}
