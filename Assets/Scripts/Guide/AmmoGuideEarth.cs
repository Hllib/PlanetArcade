using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoGuideEarth : MonoBehaviour
{
    private bool _hasEntered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_hasEntered)
        {
            _hasEntered = true;
            Guide.Instance.EnableSlime();
        }
    }
}
