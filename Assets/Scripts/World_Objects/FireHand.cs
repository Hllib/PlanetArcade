using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireHand : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _fireElements;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Destroy(gameObject, 5f);
    }

    public void SetFire()
    {
        foreach (var fireElement in _fireElements)
        {
            _spriteRenderer.enabled = false;
            fireElement.SetActive(true);
        }
    }
}
