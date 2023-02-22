using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crater : MonoBehaviour
{
    private Animator _playerAnim;
    private Player _player;

    void Start()
    {
        _playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _player = collision.GetComponent<Player>();
            StartCoroutine(Fall());
        }
        else
        {
            IDamageable hit = collision.GetComponent<IDamageable>();
            if (hit != null)
            {
                hit.Damage(100);
            }
        }
    }

    IEnumerator Fall()
    {
        yield return new WaitForSeconds(0.1f);

        _playerAnim.SetTrigger("Fall");
        _player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        _player.speed = 0f;
        _player.isDead = true;
        GameManager.Instance.IsPlayerDead = true;
        Destroy(_player.gameObject, 2f);
    }
}
