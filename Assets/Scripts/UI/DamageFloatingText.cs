using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFloatingText : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * 2.0f * Time.deltaTime);
    }

    private void Start()
    {
        Destroy(gameObject, 1.0f);    
    }
}
