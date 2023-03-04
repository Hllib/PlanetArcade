using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField]
    private GameObject _teleportPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.teleportSound, Vector3.zero);
            collision.transform.position = _teleportPoint.transform.position;
        }
    }
}
