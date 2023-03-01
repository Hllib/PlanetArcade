using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    private float _speed = 5f;
    private SpriteRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        var vector = new Vector3(1, -1);
        transform.Translate(vector* _speed * Time.deltaTime);
    }

    public void OnFall()
    {
        _speed = 0;
        _renderer.enabled = false;  
        Destroy(gameObject, 3f);
    }
}
